using MediatorCqrsApi.Dominio.Entidade;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MediatorCqrsApi.Infra.Mapeamento
{
    public class EmpresaMapeamento : IEntityTypeConfiguration<Empresa>
    {
        public void Configure(EntityTypeBuilder<Empresa> builder)
        {
            builder.ToTable("Empresa");
            builder.HasKey(s => s.Id);
            builder.Property(s => s.Id)
                .HasColumnName("Id")
                .IsRequired()
                .HasColumnType("uniqueidentifier");

            builder.Property(s => s.Referencia)
                .HasColumnName("Referencia")
                .HasColumnType("varchar(50)")
                .IsRequired(true)
                .IsUnicode(false);
            builder.Property(s => s.Descricao)
                .HasColumnName("Descricao")
                .HasColumnType("varchar(300)")
                .IsRequired(true);
            builder.Property(s => s.Inativo)
                .HasColumnName("Inativo")
                .HasColumnType("bit")
                .IsRequired(true);



        }
    }
}
