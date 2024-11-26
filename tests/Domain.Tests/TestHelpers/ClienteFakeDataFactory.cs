using Domain.Entities;
using Infra.Dto;

namespace Domain.Tests.TestHelpers;

public static class ClienteFakeDataFactory
{
    private static readonly Guid _testGuid = Guid.Parse("d290f1ee-6c54-4b01-90e6-d701748f0851");
    private static readonly Guid _outroTestGuid = Guid.Parse("a290f1ee-6c54-4b01-90e6-d701748f0853");
    private static readonly Guid _invalidoTestGuid = Guid.Parse("e290f1ee-6c54-4b01-90e6-d701748f0852");

    public static Cliente CriarClienteValido() => new(_testGuid, "João Silva", "joao@teste.com", "63641502098", true);

    public static Cliente CriarClienteComNomeInvalido() => new(_invalidoTestGuid, "J", "joao@teste.com", "63641502098", true);

    public static Cliente CriarClienteComCPFInvalido() => new(_invalidoTestGuid, "João Silva", "joao@teste.com", "11111111111", true);

    public static ClienteDb CriarClienteDbValido() => new()
    {
        Id = _testGuid,
        Nome = "João Silva",
        Email = "joao@teste.com",
        Cpf = "63641502098",
        Ativo = true
    };

    public static ClienteDb CriarOutroClienteDbValido() => new()
    {
        Id = _outroTestGuid,
        Nome = "Maria Souza",
        Email = "maria@teste.com",
        Cpf = "12345678901",
        Ativo = true
    };

    public static ClienteDb CriarClienteDbInvalido() => new()
    {
        Id = _invalidoTestGuid,
        Nome = "A",
        Email = "invalido",
        Cpf = "11111111111",
        Ativo = true
    };
}
