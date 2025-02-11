using buzzaraApi.Data;
using buzzaraApi.Models;
using Microsoft.AspNetCore.Mvc;
using BCrypt.Net;

namespace buzzaraApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UsuariosController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] Usuario usuario)
        {
            if (string.IsNullOrEmpty(usuario.Email) || string.IsNullOrEmpty(usuario.SenhaHash))
            {
                return BadRequest("Email e Senha são obrigatórios.");
            }

            var existingUser = _context.Usuarios.FirstOrDefault(u => u.Email == usuario.Email);
            if (existingUser != null)
            {
                return BadRequest("Usuário já cadastrado.");
            }

            usuario.SenhaHash = BCrypt.Net.BCrypt.HashPassword(usuario.SenhaHash);
            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Usuário cadastrado com sucesso!" });
        }
    }
}
