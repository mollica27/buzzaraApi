using buzzaraApi.Data;
using buzzaraApi.Models;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net; // Biblioteca bcrypt

namespace buzzaraApi.Services
{
    public class UsuarioService
    {
        private readonly ApplicationDbContext _context;

        public UsuarioService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Usuario> RegistrarUsuario(Usuario usuario)
        {
            // Hash da senha antes de salvar
            usuario.SenhaHash = BCrypt.Net.BCrypt.HashPassword(usuario.SenhaHash);

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();
            return usuario;
        }

        public async Task<List<Usuario>> ObterTodosUsuarios()
        {
            return await _context.Usuarios.ToListAsync();
        }

        public async Task<Usuario?> ObterUsuarioPorId(int id)
        {
            return await _context.Usuarios.FindAsync(id);
        }

        public async Task<Usuario?> AtualizarUsuario(int id, Usuario usuarioAtualizado)
        {
            var usuarioExistente = await _context.Usuarios.FindAsync(id);
            if (usuarioExistente == null)
                return null;

            // Se a senha tiver sido alterada, re-hasheie
            if (usuarioAtualizado.SenhaHash != usuarioExistente.SenhaHash)
            {
                usuarioExistente.SenhaHash = BCrypt.Net.BCrypt.HashPassword(usuarioAtualizado.SenhaHash);
            }

            usuarioExistente.Nome = usuarioAtualizado.Nome;
            usuarioExistente.Email = usuarioAtualizado.Email;
            // ... outros campos

            await _context.SaveChangesAsync();
            return usuarioExistente;
        }

        public async Task<bool> DeletarUsuario(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
                return false;

            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
