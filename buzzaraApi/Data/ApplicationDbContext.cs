using buzzaraApi.Models;
using Microsoft.EntityFrameworkCore;

namespace buzzaraApi.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Entidades já existentes
        public DbSet<Usuario> Usuarios { get; set; } = null!;
        public DbSet<PerfilAcompanhante> PerfisAcompanhantes { get; set; } = null!;
        public DbSet<Servico> Servicos { get; set; } = null!;
        public DbSet<Agendamento> Agendamentos { get; set; } = null!;

        // Novas entidades para fotos e vídeos
        public DbSet<FotoAcompanhante> FotosAcompanhantes { get; set; } = null!;
        public DbSet<VideoAcompanhante> VideosAcompanhantes { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // 1) Configuração para evitar múltiplos caminhos de exclusão em cascata em Agendamentos
            modelBuilder.Entity<Agendamento>()
                .HasOne(a => a.PerfilAcompanhante)
                .WithMany(p => p.Agendamentos)
                .HasForeignKey(a => a.PerfilAcompanhanteID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Agendamento>()
                .HasOne(a => a.Cliente)
                .WithMany(u => u.Agendamentos)
                .HasForeignKey(a => a.ClienteID)
                .OnDelete(DeleteBehavior.NoAction);

            // 2) Configuração do relacionamento entre PerfilAcompanhante e Usuario
            modelBuilder.Entity<PerfilAcompanhante>()
                .HasOne(p => p.Usuario)
                .WithMany(u => u.PerfisAcompanhantes)
                .HasForeignKey(p => p.UsuarioID)
                .OnDelete(DeleteBehavior.NoAction);

            base.OnModelCreating(modelBuilder);
        }
    }
}
