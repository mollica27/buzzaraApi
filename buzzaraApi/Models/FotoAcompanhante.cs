using System.Text.Json.Serialization;

namespace buzzaraApi.Models
{
    public class FotoAcompanhante
    {
        public int FotoAcompanhanteID { get; set; }

        // FK para PerfilAcompanhante
        public int PerfilAcompanhanteID { get; set; }
        public string UrlFoto { get; set; } = null!;
        public DateTime DataUpload { get; set; } = DateTime.Now;
    }
}
