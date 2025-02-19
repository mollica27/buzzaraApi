using buzzaraApi.Data;
using buzzaraApi.DTOs;
using buzzaraApi.Models;
using Microsoft.EntityFrameworkCore;

namespace buzzaraApi.Services
{
    public class PerfilAcompanhanteService
    {
        private readonly ApplicationDbContext _context;

        public PerfilAcompanhanteService(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Criar um novo perfil de acompanhante
        /// </summary>
        /// <param name="dto">Dados para criação do perfil</param>
        /// <returns>PerfilAcompanhante criado</returns>
        public async Task<PerfilAcompanhante> CriarPerfil(PerfilAcompanhanteDTO dto)
        {
            var perfil = new PerfilAcompanhante
            {
                UsuarioID = dto.UsuarioID,
                Descricao = dto.Descricao,
                Localizacao = dto.Localizacao,
                Tarifa = dto.Tarifa,
                DataAtualizacao = DateTime.Now
            };

            _context.PerfisAcompanhantes.Add(perfil);
            await _context.SaveChangesAsync();
            return perfil;
        }

        /// <summary>
        /// Buscar um perfil de acompanhante por ID
        /// </summary>
        /// <param name="id">ID do perfil</param>
        /// <returns>PerfilAcompanhante ou null se não encontrado</returns>
        public async Task<PerfilAcompanhante?> ObterPerfilPorId(int id)
        {
            return await _context.PerfisAcompanhantes
                .Include(p => p.Fotos)   // Apenas leitura (exibe fotos associadas)
                .Include(p => p.Videos) // Apenas leitura (exibe vídeos associados)
                .FirstOrDefaultAsync(p => p.PerfilAcompanhanteID == id);
        }

        /// <summary>
        /// Atualizar um perfil de acompanhante
        /// </summary>
        /// <param name="id">ID do perfil</param>
        /// <param name="dto">Dados atualizados</param>
        /// <returns>Perfil atualizado ou null se não encontrado</returns>
        public async Task<PerfilAcompanhante?> AtualizarPerfil(int id, PerfilAcompanhanteDTO dto)
        {
            var perfil = await _context.PerfisAcompanhantes.FindAsync(id);
            if (perfil == null)
                return null;

            perfil.Descricao = dto.Descricao;
            perfil.Localizacao = dto.Localizacao;
            perfil.Tarifa = dto.Tarifa;
            perfil.DataAtualizacao = DateTime.Now;

            await _context.SaveChangesAsync();
            return perfil;
        }

        /// <summary>
        /// Deletar um perfil de acompanhante
        /// </summary>
        /// <param name="id">ID do perfil</param>
        /// <returns>true se deletou, false se não encontrado</returns>
        public async Task<bool> DeletarPerfil(int id)
        {
            var perfil = await _context.PerfisAcompanhantes.FindAsync(id);
            if (perfil == null)
                return false;

            _context.PerfisAcompanhantes.Remove(perfil);
            await _context.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Listar todos os perfis (sem paginação)
        /// </summary>
        /// <returns>Lista de PerfilAcompanhante</returns>
        public async Task<List<PerfilAcompanhante>> ObterTodosPerfis()
        {
            return await _context.PerfisAcompanhantes
                .Include(p => p.Fotos)
                .Include(p => p.Videos)
                .ToListAsync();
        }

        /// <summary>
        /// Listar todos os perfis com paginação
        /// </summary>
        /// <param name="pageNumber">Página atual</param>
        /// <param name="pageSize">Quantidade de registros por página</param>
        /// <returns>Lista de PerfilAcompanhante</returns>
        public async Task<List<PerfilAcompanhante>> ObterTodosPerfis(int pageNumber = 1, int pageSize = 10)
        {
            return await _context.PerfisAcompanhantes
                .Include(p => p.Fotos)
                .Include(p => p.Videos)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        /// <summary>
        /// Buscar perfis por localização e faixa de tarifa
        /// </summary>
        /// <param name="localizacao">Texto a ser buscado em Localizacao</param>
        /// <param name="tarifaMin">Tarifa mínima</param>
        /// <param name="tarifaMax">Tarifa máxima</param>
        /// <returns>Lista de PerfilAcompanhante filtrada</returns>
        public async Task<List<PerfilAcompanhante>> BuscarPerfis(string? localizacao, decimal? tarifaMin, decimal? tarifaMax)
        {
            var query = _context.PerfisAcompanhantes.AsQueryable();

            if (!string.IsNullOrEmpty(localizacao))
                query = query.Where(p => p.Localizacao != null && p.Localizacao.Contains(localizacao));

            if (tarifaMin.HasValue)
                query = query.Where(p => p.Tarifa >= tarifaMin.Value);

            if (tarifaMax.HasValue)
                query = query.Where(p => p.Tarifa <= tarifaMax.Value);

            return await query
                .Include(p => p.Fotos)
                .Include(p => p.Videos)
                .ToListAsync();
        }
    }
}
