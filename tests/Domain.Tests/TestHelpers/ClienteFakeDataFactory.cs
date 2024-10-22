using Domain.Entities;
using Infra.Dto;

namespace Domain.Tests.TestHelpers
{
    public static class ClienteFakeDataFactory
    {
        private static readonly Guid _testGuid = Guid.NewGuid();

        public static Cliente CriarClienteValido() => new(_testGuid, "João Silva", "joao@teste.com", "63641502098", true);

        public static Cliente CriarClienteComNomeInvalido() => new(_testGuid, "J", "joao@teste.com", "63641502098", true);

        public static Cliente CriarClienteComCPFInvalido() => new(_testGuid, "J", "joao@teste.com", "11111111111", true);

        public static ClienteDb CriarClienteDbValido() => new()
        {
            Id = _testGuid,
            Nome = "João Silva",
            Email = "joao@teste.com",
            Cpf = "63641502098",
            Ativo = true
        };
    }
}
