namespace buzzaraApi.Models
{
    public class PerfilAcompanhante
    {
        public int PerfilAcompanhanteID { get; set; }
        public int UsuarioID { get; set; }
        public required Usuario Usuario { get; set; }
        public string Descricao { get; set; }
        public required string Localizacao { get; set; }
        public decimal Tarifa { get; set; }
        public DateTime DataAtualizacao { get; set; } = DateTime.Now;

        // Relação
        public ICollection<Agendamento> Agendamentos { get; set; } = new List<Agendamento>();
    }
}
