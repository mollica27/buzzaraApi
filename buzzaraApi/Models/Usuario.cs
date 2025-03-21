﻿namespace buzzaraApi.Models
{
    public class Usuario
    {
        public int UsuarioID { get; set; }
        public required string Nome { get; set; }
        public required string Email { get; set; }
        public required string SenhaHash { get; set; }
        public DateTime DataCadastro { get; set; } = DateTime.Now;
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiration { get; set; }

        public required string Telefone { get; set; }
        public required string Cpf { get; set; }
        public string? Genero { get; set; }

        public bool Ativo { get; set; } = true;
        public string? Role { get; set; } = "Acompanhante";

        public ICollection<PerfilAcompanhante> PerfisAcompanhantes { get; set; } = new List<PerfilAcompanhante>();
        public ICollection<Agendamento> Agendamentos { get; set; } = new List<Agendamento>();
    }
}
