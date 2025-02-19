using buzzaraApi.DTOs;
using buzzaraApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace buzzaraApi.Controllers
{
    [Route("api/midias")]
    [ApiController]
    public class MidiaController : ControllerBase
    {
        private readonly MidiaService _midiaService;

        public MidiaController(MidiaService midiaService)
        {
            _midiaService = midiaService;
        }

        // POST: api/midias/fotos
        [HttpPost("fotos")]
        public async Task<IActionResult> AdicionarFoto([FromBody] FotoAcompanhanteDTO dto)
        {
            var foto = await _midiaService.AdicionarFoto(dto);
            if (foto == null)
                return NotFound(new { message = "Perfil não encontrado." });

            return Ok(foto);
        }

        // POST: api/midias/videos
        [HttpPost("videos")]
        public async Task<IActionResult> AdicionarVideo([FromBody] VideoAcompanhanteDTO dto)
        {
            var video = await _midiaService.AdicionarVideo(dto);
            if (video == null)
                return NotFound(new { message = "Perfil não encontrado." });

            return Ok(video);
        }

        // POST: api/midias/upload-foto
        [HttpPost("upload-foto")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadFoto([FromForm] IFormFile file, [FromForm] int perfilAcompanhanteID)
        {
            var result = await _midiaService.UploadFoto(file, perfilAcompanhanteID);
            if (result == null)
                return NotFound(new { message = "Perfil não encontrado ou arquivo inválido." });

            return Ok(result);
        }

        // Aqui você pode ter métodos de deleção de foto, listagem de fotos, etc.
    }
}
