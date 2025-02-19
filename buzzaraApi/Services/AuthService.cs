using buzzaraApi.Data;
using buzzaraApi.DTOs;
using buzzaraApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using BCrypt.Net;

namespace buzzaraApi.Services
{
    public class AuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthService(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        /// <summary>
        /// Realiza o login do usuário, verificando a senha via BCrypt e gerando um token JWT + Refresh Token.
        /// </summary>
        /// <param name="loginDTO">Email e senha do usuário</param>
        /// <returns>AuthResponseDTO com Token e RefreshToken, ou null se credenciais inválidas</returns>
        public async Task<AuthResponseDTO?> Login(LoginDTO loginDTO)
        {
            // Busca o usuário pelo email
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == loginDTO.Email);

            // Verifica se o usuário existe e se a senha está correta via BCrypt
            if (usuario == null || !BCrypt.Net.BCrypt.Verify(loginDTO.Senha, usuario.SenhaHash))
            {
                return null; // Retorna null se credenciais forem inválidas
            }

            // Se estiver tudo certo, gera o token e o refresh token
            var token = GerarToken(usuario);
            var refreshToken = GerarRefreshToken();

            // Salva o refresh token no banco
            usuario.RefreshToken = refreshToken;
            usuario.RefreshTokenExpiration = DateTime.UtcNow.AddDays(7);
            await _context.SaveChangesAsync();

            return new AuthResponseDTO
            {
                Token = token,
                RefreshToken = refreshToken
            };
        }

        /// <summary>
        /// Gera o token JWT para o usuário, incluindo claims de identificação.
        /// </summary>
        /// <param name="usuario">Objeto usuário</param>
        /// <returns>Token JWT</returns>
        public string GerarToken(Usuario usuario)
        {
            // Lê as configurações de JWT do appsettings.json
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var secretKey = Encoding.UTF8.GetBytes(jwtSettings["Secret"]!);

            // Cria as claims que vão no token
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.UsuarioID.ToString()),
                new Claim(ClaimTypes.Name, usuario.Nome),
                new Claim(ClaimTypes.Email, usuario.Email)
            };

            // Configura o token JWT
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(Convert.ToDouble(jwtSettings["ExpirationMinutes"])),
                Issuer = jwtSettings["Issuer"],
                Audience = jwtSettings["Audience"],
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(secretKey),
                    SecurityAlgorithms.HmacSha256Signature
                )
            };

            // Gera o token
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        /// <summary>
        /// Gera um refresh token aleatório usando 32 bytes.
        /// </summary>
        /// <returns>Refresh Token em Base64</returns>
        public string GerarRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        /// <summary>
        /// Gera um novo par Token/RefreshToken caso o refresh token anterior ainda seja válido.
        /// </summary>
        /// <param name="refreshTokenDTO">RefreshToken atual</param>
        /// <returns>AuthResponseDTO com novo Token e RefreshToken, ou null se inválido</returns>
        public async Task<AuthResponseDTO?> RefreshToken(RefreshTokenDTO refreshTokenDTO)
        {
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.RefreshToken == refreshTokenDTO.RefreshToken);
            if (usuario == null || usuario.RefreshTokenExpiration < DateTime.UtcNow)
                return null;

            var newToken = GerarToken(usuario);
            var newRefreshToken = GerarRefreshToken();

            usuario.RefreshToken = newRefreshToken;
            usuario.RefreshTokenExpiration = DateTime.UtcNow.AddDays(7);
            await _context.SaveChangesAsync();

            return new AuthResponseDTO
            {
                Token = newToken,
                RefreshToken = newRefreshToken
            };
        }

        /// <summary>
        /// Inicia o processo de recuperação de senha, gerando um token temporário e salvando no campo RefreshToken.
        /// </summary>
        /// <param name="recuperarSenhaDTO">Email do usuário</param>
        /// <returns>True se o email existir, False caso contrário</returns>
        public async Task<bool> EnviarRecuperacaoSenha(RecuperarSenhaDTO recuperarSenhaDTO)
        {
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == recuperarSenhaDTO.Email);
            if (usuario == null)
                return false;

            // Gera um token temporário (GUID) e salva no RefreshToken
            var token = Guid.NewGuid().ToString();
            usuario.RefreshToken = token;
            usuario.RefreshTokenExpiration = DateTime.UtcNow.AddHours(1);

            await _context.SaveChangesAsync();

            // Aqui você chamaria um serviço de e-mail para enviar esse token ao usuário
            return true;
        }

        /// <summary>
        /// Redefine a senha do usuário, validando o token temporário salvo no RefreshToken.
        /// </summary>
        /// <param name="redefinirSenhaDTO">Token e nova senha</param>
        /// <returns>True se redefiniu, False se token inválido ou expirado</returns>
        public async Task<bool> RedefinirSenha(RedefinirSenhaDTO redefinirSenhaDTO)
        {
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.RefreshToken == redefinirSenhaDTO.Token);
            if (usuario == null || usuario.RefreshTokenExpiration < DateTime.UtcNow)
                return false;

            // Hashear a nova senha antes de salvar
            usuario.SenhaHash = BCrypt.Net.BCrypt.HashPassword(redefinirSenhaDTO.NovaSenha);

            // Limpa o token de recuperação
            usuario.RefreshToken = null;
            usuario.RefreshTokenExpiration = null;

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
