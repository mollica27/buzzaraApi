using buzzaraApi.Data;
using buzzaraApi.Models;
using Microsoft.EntityFrameworkCore;

namespace buzzaraApi.Services
{
    public class UsuarioService
    {
        private readonly ApplicationDbContext _context;

        public UsuarioService(ApplicationDbContext context)
        {
            _context = context;
        }

        // 📌 Criar um novo usuário
        public async Task<Usuario> RegistrarUsuario(Usuario usuario)
        {
            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();
            return usuario;
        }

        // 📌 Buscar todos os usuários
        public async Task<List<Usuario>> ObterTodosUsuarios()
        {
            return await _context.Usuarios.ToListAsync();
        }

        // 📌 Buscar um usuário por ID
        public async Task<Usuario?> ObterUsuarioPorId(int id)
        {
            return await _context.Usuarios.FindAsync(id);
        }

        // 📌 Atualizar um usuário
        public async Task<Usuario?> AtualizarUsuario(int id, Usuario usuarioAtualizado)
        {
            var usuarioExistente = await _context.Usuarios.FindAsync(id);
            if (usuarioExistente == null)
                return null;

            usuarioExistente.Nome = usuarioAtualizado.Nome;
            usuarioExistente.Email = usuarioAtualizado.Email;
            usuarioExistente.SenhaHash = usuarioAtualizado.SenhaHash;

            await _context.SaveChangesAsync();
            return usuarioExistente;
        }

        // 📌 Deletar um usuário
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
