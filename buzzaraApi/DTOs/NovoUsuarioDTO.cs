namespace buzzaraApi.DTOs
{
    public class NovoUsuarioDTO
    {
        public string NomeCompleto { get; set; } = null!;
        public string Telefone { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Cpf { get; set; } = null!;
        public string Genero { get; set; } = null!;
        public string Senha { get; set; } = null!;
        public string ConfirmaSenha { get; set; } = null!;
    }
}
