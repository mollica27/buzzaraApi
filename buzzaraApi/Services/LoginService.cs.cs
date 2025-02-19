using buzzaraApi.Data;
using buzzaraApi.DTOs;
using buzzaraApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace buzzaraApi.Services
{
    public class LoginService
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public LoginService(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<string?> Login(LoginDTO loginDTO)
        {
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == loginDTO.Email);

            if (usuario == null || usuario.SenhaHash != loginDTO.Senha) // 🚨 Aqui deve ser usado hashing na senha!
            {
                return null; // Retorna null se credenciais forem inválidas
            }

            return GerarToken(usuario);
        }

        private string GerarToken(Usuario usuario)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");

            var secretKey = Encoding.UTF8.GetBytes(jwtSettings["Secret"]!);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.UsuarioID.ToString()),
                new Claim(ClaimTypes.Name, usuario.Nome),
                new Claim(ClaimTypes.Email, usuario.Email)
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
    }
}
