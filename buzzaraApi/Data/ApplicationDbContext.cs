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

        // Definição das entidades (DbSet)
        public DbSet<Usuario> Usuarios { get; set; } = null!;
        public DbSet<PerfilAcompanhante> PerfisAcompanhantes { get; set; } = null!;
        public DbSet<Servico> Servicos { get; set; } = null!;
        public DbSet<Agendamento> Agendamentos { get; set; } = null!;

        // Configurações adicionais
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuração para evitar múltiplos caminhos de exclusão em cascata
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

            base.OnModelCreating(modelBuilder);
        }
    }
}
