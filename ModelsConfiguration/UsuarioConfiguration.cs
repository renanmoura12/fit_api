using api_fit.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace api_fit.ModelsConfiguration
{
    public class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.HasKey(a => a.Id);
            builder.Property(a => a.Email).HasMaxLength(200).IsRequired();
            builder.Property(a => a.Nome).HasMaxLength(250).IsRequired();
        }
    }
}
