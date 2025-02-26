using buzzaraApi.DTOs;
using buzzaraApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace buzzaraApi.Controllers
{
    [Route("api/novousuario")]
    [ApiController]
    public class NovoUsuarioController : ControllerBase
    {
        private readonly NovoUsuarioService _novoUsuarioService;

        public NovoUsuarioController(NovoUsuarioService novoUsuarioService)
        {
            _novoUsuarioService = novoUsuarioService;
        }

        // POST: api/novousuario/register
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Registrar([FromBody] NovoUsuarioDTO dto)
        {
            var user = await _novoUsuarioService.RegistrarNovoUsuario(dto);
            if (user == null)
                return BadRequest(new { message = "Falha ao registrar (email já em uso ou senhas não conferem)." });

            return Ok(new { message = "Usuário registrado com sucesso!" });
        }
    }
}
