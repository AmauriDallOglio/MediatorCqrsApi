using MediatorCqrsApi.Dominio.Entidade;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MediatorCqrsApi.Infra.Mapeamento
{
    public class UsuarioMapeamento : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.ToTable("Usuario");

            builder.HasKey(u => u.Id);

            builder.Property(u => u.Id)
                .HasColumnName("Id")
                .IsRequired()
                .HasColumnType("uniqueidentifier");

            builder.Property(u => u.Id_Empresa)
                .HasColumnName("Id_Empresa")
                .IsRequired()
                .HasColumnType("uniqueidentifier");

            builder.Property(u => u.Nome)
                .HasColumnName("Nome")
                .IsRequired()
                .HasColumnType("nvarchar(100)");

            builder.Property(u => u.Email)
                .HasColumnName("Email")
                .IsRequired()
                .HasColumnType("nvarchar(100)");

            builder.HasOne<Empresa>()
                .WithMany()
                .HasForeignKey(u => u.Id_Empresa)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
