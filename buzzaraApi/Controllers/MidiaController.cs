using buzzaraApi.DTOs;
using buzzaraApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace buzzaraApi.Controllers
{
    [Authorize]
    [Route("api/midias")]
    [ApiController]
    public class MidiaController : ControllerBase
    {
        private readonly MidiaService _midiaService;

        public MidiaController(MidiaService midiaService)
        {
            _midiaService = midiaService;
        }

        [HttpPost("fotos")]
        public async Task<IActionResult> AdicionarFoto([FromBody] FotoAcompanhanteDTO dto)
        {
            var foto = await _midiaService.AdicionarFoto(dto);
            if (foto == null)
                return NotFound(new { message = "Perfil não encontrado." });

            return Ok(foto);
        }

        [HttpPost("videos")]
        public async Task<IActionResult> AdicionarVideo([FromBody] VideoAcompanhanteDTO dto)
        {
            var video = await _midiaService.AdicionarVideo(dto);
            if (video == null)
                return NotFound(new { message = "Perfil não encontrado." });

            return Ok(video);
        }

        [HttpPost("upload-foto")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadFoto([FromForm] UploadFotoModel model)
        {
            // Extrair as propriedades
            var file = model.File;
            var perfilAcompanhanteID = model.PerfilAcompanhanteID;

            var result = await _midiaService.UploadFoto(file, perfilAcompanhanteID);
            if (result == null)
                return NotFound(new { message = "Perfil não encontrado ou arquivo inválido." });

            return Ok(result);
        }


        // Aqui você pode ter métodos de deleção de foto, listagem de fotos, etc.
    }
}
