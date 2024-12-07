using Controllers;
using Domain.ValueObjects;
using Gateways.Cognito.Dtos.Request;
using Gateways.Cognito.Dtos.Response;
using Gateways.Dtos.Request;
using Moq;
using UseCases;

namespace QuickFood_Backoffice.Tests.Controllers;

public class UsuarioControllerTests
{
    private readonly Mock<IUsuarioUseCase> _usuarioUseCaseMock;
    private readonly IUsuarioController _usuarioController;

    public UsuarioControllerTests()
    {
        _usuarioUseCaseMock = new Mock<IUsuarioUseCase>();
        _usuarioController = new UsuarioController(_usuarioUseCaseMock.Object);
    }

    [Fact]
    public async Task IdentificarClienteCpfAsync_DeveRetornarTokenUsuario_QuandoCredenciaisValidas()
    {
        // Arrange
        var requestDto = new ClienteIdentifiqueSeRequestDto { Cpf = "12345678901", Senha = "senha123" };
        var tokenUsuario = new TokenUsuario { AccessToken = "access_token" };
        _usuarioUseCaseMock.Setup(x => x.IdentificarClienteCpfAsync(requestDto.Cpf, requestDto.Senha, It.IsAny<CancellationToken>()))
            .ReturnsAsync(tokenUsuario);

        // Act
        var result = await _usuarioController.IdentificarClienteCpfAsync(requestDto, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(tokenUsuario.AccessToken, result?.AccessToken);
    }

    [Fact]
    public async Task IdentificarFuncionarioAsync_DeveRetornarTokenUsuario_QuandoCredenciaisValidas()
    {
        // Arrange
        var requestDto = new FuncinarioIdentifiqueSeRequestDto { Email = "usuario@teste.com", Senha = "senha123" };
        var tokenUsuario = new TokenUsuario { AccessToken = "access_token" };
        _usuarioUseCaseMock.Setup(x => x.IdentificarFuncionarioAsync(requestDto.Email, requestDto.Senha, It.IsAny<CancellationToken>()))
            .ReturnsAsync(tokenUsuario);

        // Act
        var result = await _usuarioController.IdentificarFuncionarioAsync(requestDto, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(tokenUsuario.AccessToken, result?.AccessToken);
    }

    [Fact]
    public async Task ConfirmarEmailVerificacaoAsync_DeveRetornarTrue_QuandoConfirmacaoBemSucedida()
    {
        // Arrange
        var requestDto = new ConfirmarEmailVerificacaoDto { Email = "usuario@teste.com", CodigoVerificacao = "123456" };
        _usuarioUseCaseMock.Setup(x => x.ConfirmarEmailVerificacaoAsync(It.IsAny<EmailVerificacao>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var result = await _usuarioController.ConfirmarEmailVerificacaoAsync(requestDto, CancellationToken.None);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task SolicitarRecuperacaoSenhaAsync_DeveRetornarTrue_QuandoSolicitacaoBemSucedida()
    {
        // Arrange
        var requestDto = new SolicitarRecuperacaoSenhaDto { Email = "usuario@teste.com" };
        _usuarioUseCaseMock.Setup(x => x.SolicitarRecuperacaoSenhaAsync(It.IsAny<RecuperacaoSenha>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var result = await _usuarioController.SolicitarRecuperacaoSenhaAsync(requestDto, CancellationToken.None);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task EfetuarResetSenhaAsync_DeveRetornarTrue_QuandoResetBemSucedido()
    {
        // Arrange
        var requestDto = new ResetarSenhaDto { Email = "usuario@teste.com", CodigoVerificacao = "123456", NovaSenha = "novaSenha123" };
        _usuarioUseCaseMock.Setup(x => x.EfetuarResetSenhaAsync(It.IsAny<ResetSenha>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var result = await _usuarioController.EfetuarResetSenhaAsync(requestDto, CancellationToken.None);

        // Assert
        Assert.True(result);
    }
}