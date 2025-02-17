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

        [HttpGet]
        public async Task<IActionResult> ObterUsuarios()
        {
            try
            {
                var usuarios = await _usuarioService.ObterTodosUsuarios();
                return Ok(usuarios);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObterUsuarioPorId(int id)
        {
            try
            {
                var usuario = await _usuarioService.ObterUsuarioPorId(id);
                if (usuario == null)
                    return NotFound(new { message = "Usuário não encontrado!" });

                return Ok(usuario);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizarUsuario(int id, [FromBody] Usuario usuarioAtualizado)
        {
            try
            {
                var usuario = await _usuarioService.AtualizarUsuario(id, usuarioAtualizado);
                if (usuario == null)
                    return NotFound(new { message = "Usuário não encontrado!" });

                return Ok(new { message = "Usuário atualizado com sucesso!", usuario });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletarUsuario(int id)
        {
            try
            {
                var sucesso = await _usuarioService.DeletarUsuario(id);
                if (!sucesso)
                    return NotFound(new { message = "Usuário não encontrado!" });

                return Ok(new { message = "Usuário deletado com sucesso!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
