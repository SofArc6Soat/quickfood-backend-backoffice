using Gateways.Dtos.Request;
using Infra.Dto;

namespace QuickFood_Backoffice.Tests.TestHelpers;

public static class FuncionarioFakeDataFactory
{
    public static FuncionarioDb CriarFuncionarioDbValido() => new()
    {
        Id = ObterGuid(),
        Nome = "Funcionario Exemplo",
        Email = "funcionario@exemplo.com",
        Ativo = true
    };

    public static FuncionarioDb CriarOutroFuncionarioDbValido() => new()
    {
        Id = ObterOutroGuid(),
        Nome = "Outro Funcionario Exemplo",
        Email = "outrofuncionario@exemplo.com",
        Ativo = true
    };

    public static FuncionarioDb CriarFuncionarioDbInvalido() => new()
    {
        Id = ObterGuidInvalido(),
        Nome = "A",
        Email = "invalido",
        Ativo = true
    };

    public static FuncinarioIdentifiqueSeRequestDto CriarFuncionarioIdentifiqueSeRequestValido() => new()
    {
        Email = "funcionario@teste.com",
        Senha = "SenhaTeste123"
    };

    public static FuncinarioIdentifiqueSeRequestDto CriarFuncionarioIdentifiqueSeRequestInvalido() => new()
    {
        Email = "invalido",
        Senha = "123"
    };

    public static Guid ObterGuid() => Guid.Parse("d290f1ee-6c54-4b01-90e6-d701748f0851");

    public static Guid ObterOutroGuid() => Guid.Parse("a290f1ee-6c54-4b01-90e6-d701748f0853");

    public static Guid ObterGuidInvalido() => Guid.Parse("e290f1ee-6c54-4b01-90e6-d701748f0852");
}