using Infra.Dto;
using Infra.Mappings.SeedData;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Diagnostics.CodeAnalysis;

namespace Infra.Mappings
{
    [ExcludeFromCodeCoverage]
    public class ClienteMapping : IEntityTypeConfiguration<ClienteDb>
    {
        public void Configure(EntityTypeBuilder<ClienteDb> builder)
        {
            builder.ToTable("Clientes", "dbo");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Nome)
                   .IsRequired()
                   .HasColumnType("varchar(50)");

            builder.Property(c => c.Email)
                    .IsRequired()
                   .HasColumnType("varchar(100)");

            builder.Property(c => c.Cpf)
                   .HasColumnType("varchar(11)");

            // UK
            builder.HasIndex(u => u.Email)
                   .IsUnique();

            builder.HasIndex(u => u.Cpf)
                   .IsUnique();

            // Data
            builder.HasData(ClienteSeedData.GetSeedData());
        }
    }
}