using Api.Controllers;
using Controllers;
using Core.Domain.Notificacoes;
using Core.WebApi.Controller;
using Gateways.Cognito.Dtos.Request;
using Gateways.Cognito.Dtos.Response;
using Gateways.Dtos.Request;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace API.Tests.Controllers;

public class UsuariosApiControllerTests
{
    private readonly Mock<IUsuarioController> _usuarioControllerMock;
    private readonly Mock<INotificador> _notificadorMock;
    private readonly UsuariosApiController _usuariosApiController;

    public UsuariosApiControllerTests()
    {
        _usuarioControllerMock = new Mock<IUsuarioController>();
        _notificadorMock = new Mock<INotificador>();
        _usuariosApiController = new UsuariosApiController(_usuarioControllerMock.Object, _notificadorMock.Object);
    }

    [Fact]
    public async Task IdentificarClienteCpf_DeveRetornarCreated_QuandoCredenciaisValidas()
    {
        // Arrange
        var request = new ClienteIdentifiqueSeRequestDto { Cpf = "12345678901", Senha = "senha123" };
        var tokenUsuario = new TokenUsuario { AccessToken = "access_token" };
        _usuarioControllerMock.Setup(x => x.IdentificarClienteCpfAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(tokenUsuario);

        // Act
        var result = await _usuariosApiController.IdentificarClienteCpf(request, CancellationToken.None);

        // Assert
        var createdResult = Assert.IsType<CreatedResult>(result);
        var response = Assert.IsType<BaseApiResponse>(createdResult.Value);
        Assert.True(response.Success);
        Assert.Equal(tokenUsuario, response.Data);
    }

    [Fact]
    public async Task IdentificarClienteCpf_DeveRetornarBadRequest_QuandoModelStateInvalido()
    {
        // Arrange
        var request = new ClienteIdentifiqueSeRequestDto { Cpf = "12345678901", Senha = "senha123" };
        _usuariosApiController.ModelState.AddModelError("Cpf", "Required");

        // Act
        var result = await _usuariosApiController.IdentificarClienteCpf(request, CancellationToken.None);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        var response = Assert.IsType<BaseApiResponse>(badRequestResult.Value);
        Assert.False(response.Success);
        Assert.NotNull(response.Errors);
    }

    [Fact]
    public async Task IdentificarFuncionario_DeveRetornarCreated_QuandoCredenciaisValidas()
    {
        // Arrange
        var request = new FuncinarioIdentifiqueSeRequestDto { Email = "usuario@teste.com", Senha = "senha123" };
        var tokenUsuario = new TokenUsuario { AccessToken = "access_token" };
        _usuarioControllerMock.Setup(x => x.IdentificarFuncionarioAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(tokenUsuario);

        // Act
        var result = await _usuariosApiController.IdentificarFuncionario(request, CancellationToken.None);

        // Assert
        var createdResult = Assert.IsType<CreatedResult>(result);
        var response = Assert.IsType<BaseApiResponse>(createdResult.Value);
        Assert.True(response.Success);
        Assert.Equal(tokenUsuario, response.Data);
    }

    [Fact]
    public async Task IdentificarFuncionario_DeveRetornarBadRequest_QuandoModelStateInvalido()
    {
        // Arrange
        var request = new FuncinarioIdentifiqueSeRequestDto { Email = "usuario@teste.com", Senha = "senha123" };
        _usuariosApiController.ModelState.AddModelError("Email", "Required");

        // Act
        var result = await _usuariosApiController.IdentificarFuncionario(request, CancellationToken.None);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        var response = Assert.IsType<BaseApiResponse>(badRequestResult.Value);
        Assert.False(response.Success);
        Assert.NotNull(response.Errors);
    }

    [Fact]
    public async Task ConfirmarEmailVerificaoAsync_DeveRetornarCreated_QuandoVerificacaoValida()
    {
        // Arrange
        var request = new ConfirmarEmailVerificacaoDto { Email = "usuario@teste.com", CodigoVerificacao = "123456" };
        _usuarioControllerMock.Setup(x => x.ConfirmarEmailVerificacaoAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var result = await _usuariosApiController.ConfirmarEmailVerificaoAsync(request, CancellationToken.None);

        // Assert
        var createdResult = Assert.IsType<CreatedResult>(result);
        var response = Assert.IsType<BaseApiResponse>(createdResult.Value);
        Assert.True(response.Success);
        Assert.True((bool)response.Data);
    }

    [Fact]
    public async Task ConfirmarEmailVerificaoAsync_DeveRetornarBadRequest_QuandoModelStateInvalido()
    {
        // Arrange
        var request = new ConfirmarEmailVerificacaoDto { Email = "usuario@teste.com", CodigoVerificacao = "123456" };
        _usuariosApiController.ModelState.AddModelError("Email", "Required");

        // Act
        var result = await _usuariosApiController.ConfirmarEmailVerificaoAsync(request, CancellationToken.None);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        var response = Assert.IsType<BaseApiResponse>(badRequestResult.Value);
        Assert.False(response.Success);
        Assert.NotNull(response.Errors);
    }

    [Fact]
    public async Task SolicitarRecuperacaoSenhaAsync_DeveRetornarCreated_QuandoSolicitacaoValida()
    {
        // Arrange
        var request = new SolicitarRecuperacaoSenhaDto { Email = "usuario@teste.com" };
        _usuarioControllerMock.Setup(x => x.SolicitarRecuperacaoSenhaAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var result = await _usuariosApiController.SolicitarRecuperacaoSenhaAsync(request, CancellationToken.None);

        // Assert
        var createdResult = Assert.IsType<CreatedResult>(result);
        var response = Assert.IsType<BaseApiResponse>(createdResult.Value);
        Assert.True(response.Success);
        Assert.True((bool)response.Data);
    }

    [Fact]
    public async Task SolicitarRecuperacaoSenhaAsync_DeveRetornarBadRequest_QuandoModelStateInvalido()
    {
        // Arrange
        var request = new SolicitarRecuperacaoSenhaDto { Email = "usuario@teste.com" };
        _usuariosApiController.ModelState.AddModelError("Email", "Required");

        // Act
        var result = await _usuariosApiController.SolicitarRecuperacaoSenhaAsync(request, CancellationToken.None);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        var response = Assert.IsType<BaseApiResponse>(badRequestResult.Value);
        Assert.False(response.Success);
        Assert.NotNull(response.Errors);
    }

    [Fact]
    public async Task EfetuarResetSenhaAsync_DeveRetornarCreated_QuandoResetValido()
    {
        // Arrange
        var request = new ResetarSenhaDto { Email = "usuario@teste.com", CodigoVerificacao = "123456", NovaSenha = "novaSenha123" };
        _usuarioControllerMock.Setup(x => x.EfetuarResetSenhaAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var result = await _usuariosApiController.EfetuarResetSenhaAsync(request, CancellationToken.None);

        // Assert
        var createdResult = Assert.IsType<CreatedResult>(result);
        var response = Assert.IsType<BaseApiResponse>(createdResult.Value);
        Assert.True(response.Success);
        Assert.True((bool)response.Data);
    }

    [Fact]
    public async Task EfetuarResetSenhaAsync_DeveRetornarBadRequest_QuandoModelStateInvalido()
    {
        // Arrange
        var request = new ResetarSenhaDto { Email = "usuario@teste.com", CodigoVerificacao = "123456", NovaSenha = "novaSenha123" };
        _usuariosApiController.ModelState.AddModelError("Email", "Required");

        // Act
        var result = await _usuariosApiController.EfetuarResetSenhaAsync(request, CancellationToken.None);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        var response = Assert.IsType<BaseApiResponse>(badRequestResult.Value);
        Assert.False(response.Success);
        Assert.NotNull(response.Errors);
    }
}
