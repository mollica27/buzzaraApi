using buzzaraApi.Models;
using buzzaraApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace buzzaraApi.Controllers
{
    [Route("api/usuarios")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly UsuarioService _usuarioService;

        public UsuariosController(UsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegistrarUsuario([FromBody] Usuario usuario)
        {
            try
            {
                var novoUsuario = await _usuarioService.RegistrarUsuario(usuario);
                return Ok(new { message = "Usuário cadastrado com sucesso!", usuario = novoUsuario });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
