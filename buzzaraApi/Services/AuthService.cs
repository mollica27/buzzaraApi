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
        public async Task<AuthResponseDTO?> Login(LoginDTO loginDTO)
        {
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == loginDTO.Email);
            if (usuario == null || !BCrypt.Net.BCrypt.Verify(loginDTO.Password, usuario.SenhaHash))
            {
                return null;
            }

            var token = GerarToken(usuario);
            var refreshToken = GerarRefreshToken();

            usuario.RefreshToken = refreshToken;
            usuario.RefreshTokenExpiration = DateTime.UtcNow.AddDays(7);
            await _context.SaveChangesAsync();

            return new AuthResponseDTO
            {
                // Renomeie 'Token' para 'AccessToken'
                AccessToken = token,
                RefreshToken = refreshToken,
                UserData = new
                {
                    Id = usuario.UsuarioID,
                    Nome = usuario.Nome,
                    Email = usuario.Email,
                    Role = usuario.Role,
                }
            };
        }

        /// <summary>
        /// Gera o token JWT para o usuário, incluindo a claim de Role.
        /// </summary>
        public string GerarToken(Usuario usuario)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var secretKey = Encoding.UTF8.GetBytes(jwtSettings["Secret"]!);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.UsuarioID.ToString()),
                new Claim(ClaimTypes.Name, usuario.Nome),
                new Claim(ClaimTypes.Email, usuario.Email),
                new Claim(ClaimTypes.Role, usuario.Role ?? "Acompanhante")
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(Convert.ToDouble(jwtSettings["ExpirationMinutes"])),
                Issuer = jwtSettings["Issuer"],
                Audience = jwtSettings["Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKey), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        /// <summary>
        /// Gera um refresh token aleatório usando 32 bytes.
        /// </summary>
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
        /// Renova o token utilizando o refresh token.
        /// </summary>
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
                AccessToken = newToken,
                RefreshToken = newRefreshToken,
                UserData = new
                {
                    Id = usuario.UsuarioID,
                    Nome = usuario.Nome,
                    Email = usuario.Email,
                    Role = usuario.Role,
                }
            };
        }

        /// <summary>
        /// Inicia o processo de recuperação de senha, gerando um token temporário e salvando no campo RefreshToken.
        /// </summary>
        public async Task<bool> EnviarRecuperacaoSenha(RecuperarSenhaDTO recuperarSenhaDTO)
        {
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == recuperarSenhaDTO.Email);
            if (usuario == null)
                return false;

            var token = Guid.NewGuid().ToString();
            usuario.RefreshToken = token;
            usuario.RefreshTokenExpiration = DateTime.UtcNow.AddHours(1);

            await _context.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Redefine a senha do usuário, validando o token temporário salvo no RefreshToken.
        /// </summary>
        public async Task<bool> RedefinirSenha(RedefinirSenhaDTO redefinirSenhaDTO)
        {
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.RefreshToken == redefinirSenhaDTO.Token);
            if (usuario == null || usuario.RefreshTokenExpiration < DateTime.UtcNow)
                return false;

            usuario.SenhaHash = BCrypt.Net.BCrypt.HashPassword(redefinirSenhaDTO.NovaSenha);
            usuario.RefreshToken = null;
            usuario.RefreshTokenExpiration = null;

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
