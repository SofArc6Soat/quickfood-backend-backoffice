using Api.Controllers;
using Controllers;
using Core.Domain.Notificacoes;
using Core.WebApi.Controller;
using Gateways.Dtos.Request;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace API.Tests.Controllers;

public class FuncionariosApiControllerTests
{
    private readonly Mock<IFuncionarioController> _funcionarioControllerMock;
    private readonly Mock<INotificador> _notificadorMock;
    private readonly FuncionariosApiController _funcionariosApiController;

    public FuncionariosApiControllerTests()
    {
        _funcionarioControllerMock = new Mock<IFuncionarioController>();
        _notificadorMock = new Mock<INotificador>();
        _funcionariosApiController = new FuncionariosApiController(_funcionarioControllerMock.Object, _notificadorMock.Object);
    }

    [Fact]
    public async Task CadastrarFuncionarioAsync_DeveRetornarCreated_QuandoFuncionarioValido()
    {
        // Arrange
        var request = new FuncionarioRequestDto
        {
            Id = Guid.NewGuid(),
            Nome = "Funcionario 1",
            Email = "funcionario@teste.com",
            Senha = "senha123",
            Ativo = true
        };
        _funcionarioControllerMock.Setup(x => x.CadastrarFuncionarioAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var result = await _funcionariosApiController.CadastrarFuncionarioAsync(request, CancellationToken.None);

        // Assert
        var createdResult = Assert.IsType<CreatedResult>(result);
        var response = Assert.IsType<BaseApiResponse>(createdResult.Value);
        Assert.True(response.Success);
        Assert.True((bool)response.Data);
    }

    [Fact]
    public async Task CadastrarFuncionarioAsync_DeveRetornarBadRequest_QuandoModelStateInvalido()
    {
        // Arrange
        var request = new FuncionarioRequestDto
        {
            Id = Guid.NewGuid(),
            Nome = "Funcionario 1",
            Email = "funcionario@teste.com",
            Senha = "senha123",
            Ativo = true
        };
        _funcionariosApiController.ModelState.AddModelError("Nome", "Required");

        // Act
        var result = await _funcionariosApiController.CadastrarFuncionarioAsync(request, CancellationToken.None);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        var response = Assert.IsType<BaseApiResponse>(badRequestResult.Value);
        Assert.False(response.Success);
        Assert.NotNull(response.Errors);
    }
}
