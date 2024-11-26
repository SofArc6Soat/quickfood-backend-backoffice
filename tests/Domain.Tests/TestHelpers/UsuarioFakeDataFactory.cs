using Gateways.Cognito.Dtos.Request;
using Gateways.Dtos.Request;

namespace Domain.Tests.TestHelpers;

public static class UsuarioFakeDataFactory
{
    public static ClienteIdentifiqueSeRequestDto CriarClienteIdentifiqueSeRequestValido() => new()
    {
        Cpf = "12345678901",
        Senha = "SenhaTeste123"
    };

    public static ClienteIdentifiqueSeRequestDto CriarClienteIdentifiqueSeRequestInvalido() => new()
    {
        Cpf = "123",
        Senha = "123"
    };

    public static ConfirmarEmailVerificacaoDto CriarConfirmarEmailVerificacaoRequestValido() => new()
    {
        Email = "usuario@teste.com",
        CodigoVerificacao = "123456"
    };

    public static ConfirmarEmailVerificacaoDto CriarConfirmarEmailVerificacaoRequestInvalido() => new()
    {
        Email = "invalido",
        CodigoVerificacao = "000000"
    };

    public static SolicitarRecuperacaoSenhaDto CriarSolicitarRecuperacaoSenhaRequestValido() => new()
    {
        Email = "usuario@teste.com"
    };

    public static SolicitarRecuperacaoSenhaDto CriarSolicitarRecuperacaoSenhaRequestInvalido() => new()
    {
        Email = "invalido"
    };

    public static ResetarSenhaDto CriarResetarSenhaRequestValido() => new()
    {
        Email = "usuario@teste.com",
        CodigoVerificacao = "123456",
        NovaSenha = "NovaSenhaTeste123"
    };

    public static ResetarSenhaDto CriarResetarSenhaRequestInvalido() => new()
    {
        Email = "invalido",
        CodigoVerificacao = "000000",
        NovaSenha = "123"
    };
}