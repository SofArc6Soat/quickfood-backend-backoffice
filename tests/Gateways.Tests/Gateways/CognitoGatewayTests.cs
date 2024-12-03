using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using Domain.Entities;
using Domain.ValueObjects;
using Gateways.Cognito;
using Gateways.Cognito.Configurations;
using Moq;

namespace Gateways.Tests.Gateways;

public class CognitoGatewayTests
{
    private readonly Mock<IAmazonCognitoIdentityProvider> _cognitoClientMock;
    private readonly Mock<ICognitoFactory> _cognitoFactoryMock;
    private readonly ICognitoConfig _cognitoConfig;
    private readonly CognitoGateway _cognitoGateway;

    public CognitoGatewayTests()
    {
        _cognitoClientMock = new Mock<IAmazonCognitoIdentityProvider>();
        _cognitoFactoryMock = new Mock<ICognitoFactory>();
        _cognitoConfig = new CognitoConfig("test-client-id", "test-client-secret", "us-east-1_examplepool");
        _cognitoGateway = new CognitoGateway(_cognitoClientMock.Object, _cognitoFactoryMock.Object, _cognitoConfig);
    }

    [Fact]
    public async Task ConfirmarEmailVerificacaoAsync_DeveRetornarTrue_QuandoConfirmacaoBemSucedida()
    {
        // Arrange
        var emailVerificacao = new EmailVerificacao("usuario@teste.com", "codigo123");

        _cognitoFactoryMock.Setup(x => x.CreateConfirmSignUpRequest(emailVerificacao.Email, emailVerificacao.CodigoVerificacao))
            .Returns(new ConfirmSignUpRequest());

        _cognitoClientMock.Setup(x => x.ConfirmSignUpAsync(It.IsAny<ConfirmSignUpRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ConfirmSignUpResponse { HttpStatusCode = System.Net.HttpStatusCode.OK });

        // Act
        var result = await _cognitoGateway.ConfirmarEmailVerificacaoAsync(emailVerificacao, CancellationToken.None);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task ConfirmarEmailVerificacaoAsync_DeveRetornarFalse_QuandoConfirmacaoFalhar()
    {
        // Arrange
        var emailVerificacao = new EmailVerificacao("usuario@teste.com", "codigo123");

        _cognitoFactoryMock.Setup(x => x.CreateConfirmSignUpRequest(emailVerificacao.Email, emailVerificacao.CodigoVerificacao))
            .Returns(new ConfirmSignUpRequest());

        _cognitoClientMock.Setup(x => x.ConfirmSignUpAsync(It.IsAny<ConfirmSignUpRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ConfirmSignUpResponse { HttpStatusCode = System.Net.HttpStatusCode.BadRequest });

        // Act
        var result = await _cognitoGateway.ConfirmarEmailVerificacaoAsync(emailVerificacao, CancellationToken.None);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task ConfirmarEmailVerificacaoAsync_DeveRetornarFalse_QuandoExcecaoForLancada()
    {
        // Arrange
        var emailVerificacao = new EmailVerificacao("usuario@teste.com", "codigo123");

        _cognitoFactoryMock.Setup(x => x.CreateConfirmSignUpRequest(emailVerificacao.Email, emailVerificacao.CodigoVerificacao))
            .Returns(new ConfirmSignUpRequest());

        _cognitoClientMock.Setup(x => x.ConfirmSignUpAsync(It.IsAny<ConfirmSignUpRequest>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Erro ao confirmar email"));

        // Act
        var result = await _cognitoGateway.ConfirmarEmailVerificacaoAsync(emailVerificacao, CancellationToken.None);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task SolicitarRecuperacaoSenhaAsync_DeveRetornarTrue_QuandoSolicitacaoBemSucedida()
    {
        // Arrange
        var recuperacaoSenha = new RecuperacaoSenha("usuario@teste.com");

        _cognitoFactoryMock.Setup(x => x.CreateForgotPasswordRequest(recuperacaoSenha.Email))
            .Returns(new ForgotPasswordRequest());

        _cognitoClientMock.Setup(x => x.ForgotPasswordAsync(It.IsAny<ForgotPasswordRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ForgotPasswordResponse { HttpStatusCode = System.Net.HttpStatusCode.OK });

        // Act
        var result = await _cognitoGateway.SolicitarRecuperacaoSenhaAsync(recuperacaoSenha, CancellationToken.None);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task SolicitarRecuperacaoSenhaAsync_DeveRetornarFalse_QuandoSolicitacaoFalhar()
    {
        // Arrange
        var recuperacaoSenha = new RecuperacaoSenha("usuario@teste.com");

        _cognitoFactoryMock.Setup(x => x.CreateForgotPasswordRequest(recuperacaoSenha.Email))
            .Returns(new ForgotPasswordRequest());

        _cognitoClientMock.Setup(x => x.ForgotPasswordAsync(It.IsAny<ForgotPasswordRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ForgotPasswordResponse { HttpStatusCode = System.Net.HttpStatusCode.BadRequest });

        // Act
        var result = await _cognitoGateway.SolicitarRecuperacaoSenhaAsync(recuperacaoSenha, CancellationToken.None);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task SolicitarRecuperacaoSenhaAsync_DeveRetornarFalse_QuandoExcecaoForLancada()
    {
        // Arrange
        var recuperacaoSenha = new RecuperacaoSenha("usuario@teste.com");

        _cognitoFactoryMock.Setup(x => x.CreateForgotPasswordRequest(recuperacaoSenha.Email))
            .Returns(new ForgotPasswordRequest());

        _cognitoClientMock.Setup(x => x.ForgotPasswordAsync(It.IsAny<ForgotPasswordRequest>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Erro ao solicitar recuperação de senha"));

        // Act
        var result = await _cognitoGateway.SolicitarRecuperacaoSenhaAsync(recuperacaoSenha, CancellationToken.None);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task EfetuarResetSenhaAsync_DeveRetornarTrue_QuandoResetBemSucedido()
    {
        // Arrange
        var resetSenha = new ResetSenha("usuario@teste.com", "codigo123", "novaSenha123");

        _cognitoFactoryMock.Setup(x => x.CreateConfirmForgotPasswordRequest(resetSenha.Email, resetSenha.CodigoVerificacao, resetSenha.NovaSenha))
            .Returns(new ConfirmForgotPasswordRequest());

        _cognitoClientMock.Setup(x => x.ConfirmForgotPasswordAsync(It.IsAny<ConfirmForgotPasswordRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ConfirmForgotPasswordResponse { HttpStatusCode = System.Net.HttpStatusCode.OK });

        // Act
        var result = await _cognitoGateway.EfetuarResetSenhaAsync(resetSenha, CancellationToken.None);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task EfetuarResetSenhaAsync_DeveRetornarFalse_QuandoResetFalhar()
    {
        // Arrange
        var resetSenha = new ResetSenha("usuario@teste.com", "codigo123", "novaSenha123");

        _cognitoFactoryMock.Setup(x => x.CreateConfirmForgotPasswordRequest(resetSenha.Email, resetSenha.CodigoVerificacao, resetSenha.NovaSenha))
            .Returns(new ConfirmForgotPasswordRequest());

        _cognitoClientMock.Setup(x => x.ConfirmForgotPasswordAsync(It.IsAny<ConfirmForgotPasswordRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ConfirmForgotPasswordResponse { HttpStatusCode = System.Net.HttpStatusCode.BadRequest });

        // Act
        var result = await _cognitoGateway.EfetuarResetSenhaAsync(resetSenha, CancellationToken.None);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task EfetuarResetSenhaAsync_DeveRetornarFalse_QuandoExcecaoForLancada()
    {
        // Arrange
        var resetSenha = new ResetSenha("usuario@teste.com", "codigo123", "novaSenha123");

        _cognitoFactoryMock.Setup(x => x.CreateConfirmForgotPasswordRequest(resetSenha.Email, resetSenha.CodigoVerificacao, resetSenha.NovaSenha))
            .Returns(new ConfirmForgotPasswordRequest());

        _cognitoClientMock.Setup(x => x.ConfirmForgotPasswordAsync(It.IsAny<ConfirmForgotPasswordRequest>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Erro ao efetuar reset de senha"));

        // Act
        var result = await _cognitoGateway.EfetuarResetSenhaAsync(resetSenha, CancellationToken.None);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task CriarUsuarioClienteAsync_DeveRetornarFalse_QuandoCpfOuEmailJaExistem()
    {
        // Arrange
        var cliente = new Cliente(Guid.NewGuid(), "Cliente Teste", "cliente@teste.com", "12345678901", true);
        var senha = "senha123";

        _cognitoClientMock.Setup(x => x.ListUsersAsync(It.IsAny<ListUsersRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ListUsersResponse { Users = new List<UserType> { new UserType() } });

        // Act
        var result = await _cognitoGateway.CriarUsuarioClienteAsync(cliente, senha, CancellationToken.None);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task CriarUsuarioClienteAsync_DeveRetornarTrue_QuandoUsuarioCriadoComSucesso()
    {
        // Arrange
        var cliente = new Cliente(Guid.NewGuid(), "Cliente Teste", "cliente@teste.com", "12345678901", true);
        var senha = "senha123";

        _cognitoClientMock.Setup(x => x.ListUsersAsync(It.IsAny<ListUsersRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ListUsersResponse { Users = new List<UserType>() });

        _cognitoFactoryMock.Setup(x => x.CreateSignUpRequest(cliente.Email, senha, cliente.Nome, cliente.Cpf))
            .Returns(new SignUpRequest());

        _cognitoFactoryMock.Setup(x => x.CreateAddUserToGroupRequest(cliente.Email, "cliente"))
            .Returns(new AdminAddUserToGroupRequest());

        _cognitoClientMock.Setup(x => x.SignUpAsync(It.IsAny<SignUpRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new SignUpResponse { HttpStatusCode = System.Net.HttpStatusCode.OK });

        _cognitoClientMock.Setup(x => x.AdminAddUserToGroupAsync(It.IsAny<AdminAddUserToGroupRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new AdminAddUserToGroupResponse { HttpStatusCode = System.Net.HttpStatusCode.OK });

        // Act
        var result = await _cognitoGateway.CriarUsuarioClienteAsync(cliente, senha, CancellationToken.None);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task CriarUsuarioClienteAsync_DeveRetornarFalse_QuandoExcecaoForLancada()
    {
        // Arrange
        var cliente = new Cliente(Guid.NewGuid(), "Cliente Teste", "cliente@teste.com", "12345678901", true);
        var senha = "senha123";

        _cognitoClientMock.Setup(x => x.ListUsersAsync(It.IsAny<ListUsersRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ListUsersResponse { Users = new List<UserType>() });

        _cognitoFactoryMock.Setup(x => x.CreateSignUpRequest(cliente.Email, senha, cliente.Nome, cliente.Cpf))
            .Returns(new SignUpRequest());

        _cognitoFactoryMock.Setup(x => x.CreateAddUserToGroupRequest(cliente.Email, "cliente"))
            .Returns(new AdminAddUserToGroupRequest());

        _cognitoClientMock.Setup(x => x.SignUpAsync(It.IsAny<SignUpRequest>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Erro ao criar usuário"));

        // Act
        var result = await _cognitoGateway.CriarUsuarioClienteAsync(cliente, senha, CancellationToken.None);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task CriarUsuarioFuncionarioAsync_DeveRetornarFalse_QuandoEmailJaExiste()
    {
        // Arrange
        var funcionario = new Funcionario(Guid.NewGuid(), "Funcionario Teste", "funcionario@teste.com", true);
        var senha = "senha123";

        _cognitoClientMock.Setup(x => x.ListUsersAsync(It.IsAny<ListUsersRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ListUsersResponse { Users = new List<UserType> { new UserType() } });

        // Act
        var result = await _cognitoGateway.CriarUsuarioFuncionarioAsync(funcionario, senha, CancellationToken.None);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task CriarUsuarioFuncionarioAsync_DeveRetornarTrue_QuandoUsuarioCriadoComSucesso()
    {
        // Arrange
        var funcionario = new Funcionario(Guid.NewGuid(), "Funcionario Teste", "funcionario@teste.com", true);
        var senha = "senha123";

        _cognitoClientMock.Setup(x => x.ListUsersAsync(It.IsAny<ListUsersRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ListUsersResponse { Users = new List<UserType>() });

        _cognitoFactoryMock.Setup(x => x.CreateSignUpRequest(funcionario.Email, senha, funcionario.Nome, null))
            .Returns(new SignUpRequest());

        _cognitoFactoryMock.Setup(x => x.CreateAddUserToGroupRequest(funcionario.Email, "admin"))
            .Returns(new AdminAddUserToGroupRequest());

        _cognitoClientMock.Setup(x => x.SignUpAsync(It.IsAny<SignUpRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new SignUpResponse { HttpStatusCode = System.Net.HttpStatusCode.OK });

        _cognitoClientMock.Setup(x => x.AdminAddUserToGroupAsync(It.IsAny<AdminAddUserToGroupRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new AdminAddUserToGroupResponse { HttpStatusCode = System.Net.HttpStatusCode.OK });

        // Act
        var result = await _cognitoGateway.CriarUsuarioFuncionarioAsync(funcionario, senha, CancellationToken.None);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task CriarUsuarioFuncionarioAsync_DeveRetornarFalse_QuandoExcecaoForLancada()
    {
        // Arrange
        var funcionario = new Funcionario(Guid.NewGuid(), "Funcionario Teste", "funcionario@teste.com", true);
        var senha = "senha123";

        _cognitoClientMock.Setup(x => x.ListUsersAsync(It.IsAny<ListUsersRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ListUsersResponse { Users = new List<UserType>() });

        _cognitoFactoryMock.Setup(x => x.CreateSignUpRequest(funcionario.Email, senha, funcionario.Nome, null))
            .Returns(new SignUpRequest());

        _cognitoFactoryMock.Setup(x => x.CreateAddUserToGroupRequest(funcionario.Email, "admin"))
            .Returns(new AdminAddUserToGroupRequest());

        _cognitoClientMock.Setup(x => x.SignUpAsync(It.IsAny<SignUpRequest>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Erro ao criar usuário"));

        // Act
        var result = await _cognitoGateway.CriarUsuarioFuncionarioAsync(funcionario, senha, CancellationToken.None);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task DeletarUsuarioCognitoAsync_DeveRetornarTrue_QuandoUsuarioDeletadoComSucesso()
    {
        // Arrange
        var email = "usuario@teste.com";

        _cognitoFactoryMock.Setup(x => x.CreateListUsersRequestByEmail(It.IsAny<string>(), email))
            .Returns(new ListUsersRequest());

        _cognitoClientMock.Setup(x => x.ListUsersAsync(It.IsAny<ListUsersRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ListUsersResponse { Users = new List<UserType> { new UserType { Username = "usuario" } } });

        _cognitoFactoryMock.Setup(x => x.CreateAdminDeleteUserRequest(It.IsAny<string>(), "usuario"))
            .Returns(new AdminDeleteUserRequest());

        _cognitoClientMock.Setup(x => x.AdminDeleteUserAsync(It.IsAny<AdminDeleteUserRequest>(), It.IsAny<CancellationToken>()));

        // Act
        var result = await _cognitoGateway.DeletarUsuarioCognitoAsync(email, CancellationToken.None);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task DeletarUsuarioCognitoAsync_DeveRetornarFalse_QuandoUsuarioNaoEncontrado()
    {
        // Arrange
        var email = "usuario@teste.com";

        _cognitoFactoryMock.Setup(x => x.CreateListUsersRequestByEmail(It.IsAny<string>(), email))
            .Returns(new ListUsersRequest());

        _cognitoClientMock.Setup(x => x.ListUsersAsync(It.IsAny<ListUsersRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ListUsersResponse { Users = new List<UserType>() });

        // Act
        var result = await _cognitoGateway.DeletarUsuarioCognitoAsync(email, CancellationToken.None);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task DeletarUsuarioCognitoAsync_DeveRetornarFalse_QuandoExcecaoForLancada()
    {
        // Arrange
        var email = "usuario@teste.com";

        _cognitoFactoryMock.Setup(x => x.CreateListUsersRequestByEmail(It.IsAny<string>(), email))
            .Returns(new ListUsersRequest());

        _cognitoClientMock.Setup(x => x.ListUsersAsync(It.IsAny<ListUsersRequest>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Erro ao deletar usuário"));

        // Act
        var result = await _cognitoGateway.DeletarUsuarioCognitoAsync(email, CancellationToken.None);

        // Assert
        Assert.False(result);
    }
}