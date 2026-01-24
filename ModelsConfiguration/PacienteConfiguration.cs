using api_fit.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace api_fit.ModelsConfiguration
{
    public class PacienteConfiguration : IEntityTypeConfiguration<Paciente>
    {
        public void Configure(EntityTypeBuilder<Paciente> builder)
        {
            builder.HasKey(a => a.Id);
            builder.Property(a => a.Nome).HasMaxLength(250).IsRequired();
            builder.Property(a => a.DataNascimento).IsRequired();
            builder.Property(a => a.NomeMae).HasMaxLength(250).IsRequired();

        }
    }
}
