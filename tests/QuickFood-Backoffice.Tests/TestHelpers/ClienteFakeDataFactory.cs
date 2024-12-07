using Domain.Entities;
using Infra.Dto;

namespace QuickFood_Backoffice.Tests.TestHelpers;

public static class ClienteFakeDataFactory
{
    public static Cliente CriarClienteValido() => new(ObterGuid(), "João Silva", "joao@teste.com", "63641502098", true);

    public static Cliente CriarClienteComNomeInvalido() => new(ObterOutroGuid(), "J", "joao@teste.com", "63641502098", true);

    public static Cliente CriarClienteComCPFInvalido() => new(ObterOutroGuid(), "João Silva", "joao@teste.com", "11111111111", true);

    public static ClienteDb CriarClienteDbValido() => new()
    {
        Id = ObterGuid(),
        Nome = "João Silva",
        Email = "joao@teste.com",
        Cpf = "63641502098",
        Ativo = true
    };

    public static ClienteDb CriarOutroClienteDbValido() => new()
    {
        Id = ObterOutroGuid(),
        Nome = "Maria Souza",
        Email = "maria@teste.com",
        Cpf = "12345678901",
        Ativo = true
    };

    public static ClienteDb CriarClienteDbInvalido() => new()
    {
        Id = ObterOutroGuid(),
        Nome = "A",
        Email = "invalido",
        Cpf = "11111111111",
        Ativo = true
    };

    public static Guid ObterGuid() => Guid.Parse("d290f1ee-6c54-4b01-90e6-d701748f0851");

    public static Guid ObterOutroGuid() => Guid.Parse("a290f1ee-6c54-4b01-90e6-d701748f0853");

    public static Guid ObterGuidInvalido() => Guid.Parse("e290f1ee-6c54-4b01-90e6-d701748f0852");
}