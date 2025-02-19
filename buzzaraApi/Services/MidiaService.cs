using buzzaraApi.Data;
using buzzaraApi.DTOs;
using buzzaraApi.Models;
using Microsoft.EntityFrameworkCore;

namespace buzzaraApi.Services
{
    public class MidiaService
    {
        private readonly ApplicationDbContext _context;

        public MidiaService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<FotoAcompanhante?> AdicionarFoto(FotoAcompanhanteDTO dto)
        {
            var perfil = await _context.PerfisAcompanhantes.FindAsync(dto.PerfilAcompanhanteID);
            if (perfil == null) return null;

            var foto = new FotoAcompanhante
            {
                PerfilAcompanhanteID = dto.PerfilAcompanhanteID,
                UrlFoto = dto.UrlFoto,
                DataUpload = DateTime.Now
            };

            _context.FotosAcompanhantes.Add(foto);
            await _context.SaveChangesAsync();
            return foto;
        }

        public async Task<VideoAcompanhante?> AdicionarVideo(VideoAcompanhanteDTO dto)
        {
            var perfil = await _context.PerfisAcompanhantes.FindAsync(dto.PerfilAcompanhanteID);
            if (perfil == null) return null;

            var video = new VideoAcompanhante
            {
                PerfilAcompanhanteID = dto.PerfilAcompanhanteID,
                UrlVideo = dto.UrlVideo,
                DataUpload = DateTime.Now
            };

            _context.VideosAcompanhantes.Add(video);
            await _context.SaveChangesAsync();
            return video;
        }

        public async Task<FotoAcompanhante?> UploadFoto(IFormFile file, int perfilAcompanhanteID)
        {
            if (file == null || file.Length == 0)
                return null;

            // Verifica se o perfil existe
            var perfil = await _context.PerfisAcompanhantes.FindAsync(perfilAcompanhanteID);
            if (perfil == null) return null;

            // Caminho onde o arquivo será salvo
            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads", "Fotos");
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(folderPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // Salvar o registro da foto no banco
            var foto = new FotoAcompanhante
            {
                PerfilAcompanhanteID = perfilAcompanhanteID,
                UrlFoto = $"/Uploads/Fotos/{fileName}",
                DataUpload = DateTime.Now
            };

            _context.FotosAcompanhantes.Add(foto);
            await _context.SaveChangesAsync();

            return foto;
        }
    }
}
