using Infra.Dto;
using Infra.Mappings.SeedData;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Diagnostics.CodeAnalysis;

namespace Infra.Mappings
{
    [ExcludeFromCodeCoverage]
    public class FuncionarioMapping : IEntityTypeConfiguration<FuncionarioDb>
    {
        public void Configure(EntityTypeBuilder<FuncionarioDb> builder)
        {
            builder.ToTable("Funcionarios", "dbo");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Nome)
                   .IsRequired()
                   .HasColumnType("varchar(50)");

            builder.Property(c => c.Email)
                   .IsRequired()
                   .HasColumnType("varchar(100)");

            // UK
            builder.HasIndex(u => u.Email)
                   .IsUnique();

            // Data
            builder.HasData(FuncionarioSeedData.GetSeedData());
        }
    }
}