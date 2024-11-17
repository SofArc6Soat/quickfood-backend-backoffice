using Infra.Dto;
using System.Diagnostics.CodeAnalysis;

namespace Infra.Mappings.SeedData
{
    [ExcludeFromCodeCoverage]
    public static class FuncionarioSeedData
    {
        public static List<FuncionarioDb> GetSeedData() =>
        [
            new FuncionarioDb {
                Id = Guid.Parse("34a86719-0082-4ef5-a620-35d55f076c31"),
                Nome = "Mario",
                Email = "sof.arc.6soat@gmail.com",
                Ativo = true
            }
        ];
    }
}