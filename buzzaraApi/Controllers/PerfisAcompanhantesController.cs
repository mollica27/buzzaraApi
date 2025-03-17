using buzzaraApi.DTOs;
using buzzaraApi.Models;
using buzzaraApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace buzzaraApi.Controllers
{
    [Route("/perfis")]
    [ApiController]
    [Authorize]
    public class PerfisAcompanhantesController : ControllerBase
    {
        private readonly PerfilAcompanhanteService _perfilService;

        public PerfisAcompanhantesController(PerfilAcompanhanteService perfilService)
        {
            _perfilService = perfilService;
        }

        // POST: api/perfis
        [HttpPost]
        public async Task<IActionResult> CriarPerfil([FromBody] PerfilAcompanhanteDTO dto)
        {
            var perfil = await _perfilService.CriarPerfil(dto);
            return Ok(perfil);
        }

        // GET: api/perfis/all
        [HttpGet("all")]
        public async Task<IActionResult> ObterTodosPerfis()
        {
            var perfis = await _perfilService.ObterTodosPerfis();
            return Ok(perfis);
        }

        // GET: api/perfis/5
        [HttpGet("{id}")]
        public async Task<IActionResult> ObterPerfilPorId(int id)
        {
            var perfil = await _perfilService.ObterPerfilPorId(id);
            if (perfil == null)
                return NotFound(new { message = "Perfil não encontrado!" });

            return Ok(perfil);
        }

        // PUT: api/perfis/5
        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizarPerfil(int id, [FromBody] PerfilAcompanhanteDTO dto)
        {
            var perfil = await _perfilService.AtualizarPerfil(id, dto);
            if (perfil == null)
                return NotFound(new { message = "Perfil não encontrado!" });

            return Ok(perfil);
        }

        // DELETE: api/perfis/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletarPerfil(int id)
        {
            var sucesso = await _perfilService.DeletarPerfil(id);
            if (!sucesso)
                return NotFound(new { message = "Perfil não encontrado!" });

            return Ok(new { message = "Perfil deletado com sucesso!" });
        }
    }
}
