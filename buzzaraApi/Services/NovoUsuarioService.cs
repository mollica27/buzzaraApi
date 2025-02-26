using buzzaraApi.Data;
using buzzaraApi.DTOs;
using buzzaraApi.Models;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;

namespace buzzaraApi.Services
{
    public class NovoUsuarioService
    {
        private readonly ApplicationDbContext _context;

        public NovoUsuarioService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Usuario?> RegistrarNovoUsuario(NovoUsuarioDTO dto)
        {
            // Verifica se as senhas conferem
            if (dto.Senha != dto.ConfirmaSenha)
                return null; // ou lance exceção

            // Verifica se o email já está em uso
            var existingUser = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Email == dto.Email);
            if (existingUser != null)
                return null; // ou retorne algo indicando falha

            // Criar o usuário
            var user = new Usuario
            {
                Nome = dto.NomeCompleto,
                Email = dto.Email,
                Telefone = dto.Telefone,
                Cpf = dto.Cpf,
                Genero = dto.Genero,
                // Hash da senha
                SenhaHash = BCrypt.Net.BCrypt.HashPassword(dto.Senha),
                DataCadastro = DateTime.Now,
                // Padrões
                Ativo = true,
                Role = "Acompanhante"
            };

            _context.Usuarios.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }
    }
}
