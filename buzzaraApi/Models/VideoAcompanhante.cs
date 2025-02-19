namespace buzzaraApi.Models
{
    public class VideoAcompanhante
    {
        public int VideoAcompanhanteID { get; set; }
        public int PerfilAcompanhanteID { get; set; }
        public string UrlVideo { get; set; } = null!;
        public DateTime DataUpload { get; set; } = DateTime.Now;
    }
}
