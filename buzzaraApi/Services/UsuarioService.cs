using buzzaraApi.Data;
using buzzaraApi.Models;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;

namespace buzzaraApi.Services
{
    public class UsuarioService
    {
        private readonly ApplicationDbContext _context;

        public UsuarioService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Usuario?> RegistrarUsuario(Usuario usuario)
        {
            // Verifica se o email já está cadastrado
            if (await _context.Usuarios.AnyAsync(u => u.Email == usuario.Email))
            {
                throw new Exception("E-mail já está cadastrado.");
            }

            // Criptografa a senha antes de salvar
            usuario.SenhaHash = BCrypt.Net.BCrypt.HashPassword(usuario.SenhaHash);

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();
            return usuario;
        }

        public async Task<Usuario?> BuscarPorEmail(string email)
        {
            return await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == email);
        }
    }
}
