namespace buzzaraApi.Models
{
    public class Servico
    {
        public int ServicoID { get; set; }
        public required string Nome { get; set; }
        public required string Descricao { get; set; }
        public decimal Preco { get; set; }
    }
}
