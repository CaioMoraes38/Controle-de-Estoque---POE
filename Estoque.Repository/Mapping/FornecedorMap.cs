using Estoque.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Estoque.Repository.Mapping
{
    public class FornecedorMap : IEntityTypeConfiguration<Fornecedor>
    {
        public void Configure(EntityTypeBuilder<Fornecedor> builder)
        {
            builder.ToTable("Fornecedor");

            builder.HasKey(prop => prop.Id);

            builder.Property(prop => prop.Nome)
                .IsRequired()
                .HasColumnType("varchar(100)");

            builder.Property(prop => prop.Endereco)
                .IsRequired()
                .HasColumnType("varchar(100)");

            builder.Property(prop => prop.Bairro)
                .IsRequired()
                .HasColumnType("varchar(100)");

            builder.Property(prop => prop.Cidade)
             .IsRequired()
             .HasColumnType("varchar(100)");

        }
    }
}
