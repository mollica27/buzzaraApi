using System.Text.Json.Serialization;

namespace buzzaraApi.Models
{
    public class PerfilAcompanhante
    {
        public int PerfilAcompanhanteID { get; set; }

        // FK para usuário (relaciona com a tabela Usuarios)
        public int UsuarioID { get; set; }
        public Usuario Usuario { get; set; } = null!;

        public string? Descricao { get; set; }
        public string? Localizacao { get; set; }
        public decimal Tarifa { get; set; }
        public DateTime DataAtualizacao { get; set; } = DateTime.Now;

        // Relações (1 -> N)
        public ICollection<Agendamento> Agendamentos { get; set; } = new List<Agendamento>();
        [JsonIgnore]
        public ICollection<FotoAcompanhante> Fotos { get; set; } = new List<FotoAcompanhante>();
        [JsonIgnore]
        public ICollection<VideoAcompanhante> Videos { get; set; } = new List<VideoAcompanhante>();
    }
}
