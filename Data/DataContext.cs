using api_fit.Models;
using Microsoft.EntityFrameworkCore;

namespace api_fit.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Dados> Dados { get; set; }
        public DbSet<Exercicio> Exercicio { get; set; }
        public DbSet<Treino> Treino { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Usuario>()
                .HasOne(u => u.Dados)
                .WithOne(d => d.Usuario)
                .HasForeignKey<Dados>(d => d.UserId).IsRequired(false);
            
            builder.ApplyConfigurationsFromAssembly(typeof(DataContext).Assembly);
        }

    }
}
