using Core.Domain.Notificacoes;
using Domain.ValueObjects;
using Gateways.Cognito;
using Gateways.Cognito.Dtos.Response;
using Moq;
using UseCases;

namespace QuickFood_Backoffice.Tests.Domain.Entities.UseCases;

public class UsuarioUseCaseTests
{
    private readonly Mock<ICognitoGateway> _cognitoGatewayMock;
    private readonly Mock<INotificador> _notificadorMock;
    private readonly IUsuarioUseCase _usuarioUseCase;

    public UsuarioUseCaseTests()
    {
        _cognitoGatewayMock = new Mock<ICognitoGateway>();
        _notificadorMock = new Mock<INotificador>();
        _usuarioUseCase = new UsuarioUseCase(_cognitoGatewayMock.Object, _notificadorMock.Object);
    }

    [Fact]
    public async Task IdentificarFuncionarioAsync_DeveRetornarTokenUsuario_QuandoCredenciaisValidas()
    {
        // Arrange
        var email = "funcionario@example.com";
        var senha = "senha123";
        var expiry = DateTimeOffset.UtcNow.AddHours(1);
        var tokenUsuario = new TokenUsuario { Email = email, AccessToken = "access_token", RefreshToken = "refresh_token", Expiry = expiry };

        _cognitoGatewayMock.Setup(x => x.IdentifiqueSeAsync(email, null, senha, It.IsAny<CancellationToken>()))
            .ReturnsAsync(tokenUsuario);

        // Act
        var result = await _usuarioUseCase.IdentificarFuncionarioAsync(email, senha, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(tokenUsuario.Email, result?.Email);
        Assert.Equal(tokenUsuario.Expiry, result?.Expiry);
        _cognitoGatewayMock.Verify(x => x.IdentifiqueSeAsync(email, null, senha, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task IdentificarClienteCpfAsync_DeveRetornarTokenUsuario_QuandoCredenciaisValidas()
    {
        // Arrange
        var cpf = "12345678900";
        var senha = "senha123";
        var expiry = DateTimeOffset.UtcNow.AddHours(1);
        var tokenUsuario = new TokenUsuario { Cpf = cpf, AccessToken = "access_token", RefreshToken = "refresh_token", Expiry = expiry };

        _cognitoGatewayMock.Setup(x => x.IdentifiqueSeAsync(null, cpf, senha, It.IsAny<CancellationToken>()))
            .ReturnsAsync(tokenUsuario);

        // Act
        var result = await _usuarioUseCase.IdentificarClienteCpfAsync(cpf, senha, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(tokenUsuario.Cpf, result?.Cpf);
        Assert.Equal(tokenUsuario.Expiry, result?.Expiry);
        _cognitoGatewayMock.Verify(x => x.IdentifiqueSeAsync(null, cpf, senha, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task ConfirmarEmailVerificacaoAsync_DeveRetornarTrue_QuandoConfirmacaoBemSucedida()
    {
        // Arrange
        var emailVerificacao = new EmailVerificacao("usuario@example.com", "codigo123");

        _cognitoGatewayMock.Setup(x => x.ConfirmarEmailVerificacaoAsync(emailVerificacao, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var result = await _usuarioUseCase.ConfirmarEmailVerificacaoAsync(emailVerificacao, CancellationToken.None);

        // Assert
        Assert.True(result);
        _cognitoGatewayMock.Verify(x => x.ConfirmarEmailVerificacaoAsync(emailVerificacao, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task ConfirmarEmailVerificacaoAsync_DeveRetornarFalse_QuandoConfirmacaoFalhar()
    {
        // Arrange
        var emailVerificacao = new EmailVerificacao("usuario@example.com", "codigo123");

        _cognitoGatewayMock.Setup(x => x.ConfirmarEmailVerificacaoAsync(emailVerificacao, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        // Act
        var result = await _usuarioUseCase.ConfirmarEmailVerificacaoAsync(emailVerificacao, CancellationToken.None);

        // Assert
        Assert.False(result);
        _cognitoGatewayMock.Verify(x => x.ConfirmarEmailVerificacaoAsync(emailVerificacao, It.IsAny<CancellationToken>()), Times.Once);
        _notificadorMock.Verify(x => x.Handle(It.IsAny<Notificacao>()), Times.Once);
    }

    [Fact]
    public async Task SolicitarRecuperacaoSenhaAsync_DeveRetornarTrue_QuandoSolicitacaoBemSucedida()
    {
        // Arrange
        var recuperacaoSenha = new RecuperacaoSenha("usuario@example.com");

        _cognitoGatewayMock.Setup(x => x.SolicitarRecuperacaoSenhaAsync(recuperacaoSenha, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var result = await _usuarioUseCase.SolicitarRecuperacaoSenhaAsync(recuperacaoSenha, CancellationToken.None);

        // Assert
        Assert.True(result);
        _cognitoGatewayMock.Verify(x => x.SolicitarRecuperacaoSenhaAsync(recuperacaoSenha, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task SolicitarRecuperacaoSenhaAsync_DeveRetornarFalse_QuandoSolicitacaoFalhar()
    {
        // Arrange
        var recuperacaoSenha = new RecuperacaoSenha("usuario@example.com");

        _cognitoGatewayMock.Setup(x => x.SolicitarRecuperacaoSenhaAsync(recuperacaoSenha, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        // Act
        var result = await _usuarioUseCase.SolicitarRecuperacaoSenhaAsync(recuperacaoSenha, CancellationToken.None);

        // Assert
        Assert.False(result);
        _cognitoGatewayMock.Verify(x => x.SolicitarRecuperacaoSenhaAsync(recuperacaoSenha, It.IsAny<CancellationToken>()), Times.Once);
        _notificadorMock.Verify(x => x.Handle(It.IsAny<Notificacao>()), Times.Once);
    }

    [Fact]
    public async Task EfetuarResetSenhaAsync_DeveRetornarTrue_QuandoResetBemSucedido()
    {
        // Arrange
        var resetSenha = new ResetSenha("usuario@example.com", "codigo123", "novaSenha123");

        _cognitoGatewayMock.Setup(x => x.EfetuarResetSenhaAsync(resetSenha, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var result = await _usuarioUseCase.EfetuarResetSenhaAsync(resetSenha, CancellationToken.None);

        // Assert
        Assert.True(result);
        _cognitoGatewayMock.Verify(x => x.EfetuarResetSenhaAsync(resetSenha, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task EfetuarResetSenhaAsync_DeveRetornarFalse_QuandoResetFalhar()
    {
        // Arrange
        var resetSenha = new ResetSenha("usuario@example.com", "codigo123", "novaSenha123");

        _cognitoGatewayMock.Setup(x => x.EfetuarResetSenhaAsync(resetSenha, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        // Act
        var result = await _usuarioUseCase.EfetuarResetSenhaAsync(resetSenha, CancellationToken.None);

        // Assert
        Assert.False(result);
        _cognitoGatewayMock.Verify(x => x.EfetuarResetSenhaAsync(resetSenha, It.IsAny<CancellationToken>()), Times.Once);
        _notificadorMock.Verify(x => x.Handle(It.IsAny<Notificacao>()), Times.Once);
    }
}