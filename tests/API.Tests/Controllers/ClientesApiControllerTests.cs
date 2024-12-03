using Api.Controllers;
using Controllers;
using Core.Domain.Notificacoes;
using Core.WebApi.Controller;
using Domain.Entities;
using Gateways.Dtos.Request;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace API.Tests.Controllers;

public class ClientesApiControllerTests
{
    private readonly Mock<IClienteController> _clienteControllerMock;
    private readonly Mock<INotificador> _notificadorMock;
    private readonly ClientesApiController _clientesApiController;

    public ClientesApiControllerTests()
    {
        _clienteControllerMock = new Mock<IClienteController>();
        _notificadorMock = new Mock<INotificador>();
        _clientesApiController = new ClientesApiController(_clienteControllerMock.Object, _notificadorMock.Object);
    }

    [Fact]
    public async Task ObterTodosClientes_DeveRetornarOk_QuandoExistemClientes()
    {
        // Arrange
        var clientes = new List<Cliente> { new Cliente(Guid.NewGuid(), "Cliente 1", "cliente@teste.com", "12345678901", true) };
        _clienteControllerMock.Setup(x => x.ObterTodosClientesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(clientes);

        // Act
        var result = await _clientesApiController.ObterTodosClientes(CancellationToken.None);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var response = Assert.IsType<BaseApiResponse>(okResult.Value);
        Assert.True(response.Success);
        Assert.Equal(clientes, response.Data);
    }

    [Fact]
    public async Task CadastrarCliente_DeveRetornarCreated_QuandoClienteValido()
    {
        // Arrange
        var request = new ClienteRequestDto
        {
            Id = Guid.NewGuid(),
            Nome = "Cliente 1",
            Email = "cliente@teste.com",
            Cpf = "12345678901",
            Senha = "senha123",
            Ativo = true
        };
        _clienteControllerMock.Setup(x => x.CadastrarClienteAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var result = await _clientesApiController.CadastrarCliente(request, CancellationToken.None);

        // Assert
        var createdResult = Assert.IsType<CreatedResult>(result);
        var response = Assert.IsType<BaseApiResponse>(createdResult.Value);
        Assert.True(response.Success);
        Assert.True((bool)response.Data);
    }

    [Fact]
    public async Task CadastrarCliente_DeveRetornarBadRequest_QuandoModelStateInvalido()
    {
        // Arrange
        var request = new ClienteRequestDto
        {
            Id = Guid.NewGuid(),
            Nome = "Cliente 1",
            Email = "cliente@teste.com",
            Cpf = "12345678901",
            Senha = "senha123",
            Ativo = true
        };
        _clientesApiController.ModelState.AddModelError("Nome", "Required");

        // Act
        var result = await _clientesApiController.CadastrarCliente(request, CancellationToken.None);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        var response = Assert.IsType<BaseApiResponse>(badRequestResult.Value);
        Assert.False(response.Success);
        Assert.NotNull(response.Errors);
    }

    [Fact]
    public async Task AtualizarCliente_DeveRetornarOk_QuandoClienteValido()
    {
        // Arrange
        var request = new ClienteAtualizarRequestDto
        {
            Id = Guid.NewGuid(),
            Nome = "Cliente 1",
            Ativo = true
        };
        _clienteControllerMock.Setup(x => x.AtualizarClienteAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var result = await _clientesApiController.AtualizarCliente(request.Id, request, CancellationToken.None);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var response = Assert.IsType<BaseApiResponse>(okResult.Value);
        Assert.True(response.Success);
        Assert.True((bool)response.Data);
    }

    [Fact]
    public async Task AtualizarCliente_DeveRetornarBadRequest_QuandoModelStateInvalido()
    {
        // Arrange
        var request = new ClienteAtualizarRequestDto
        {
            Id = Guid.NewGuid(),
            Nome = "Cliente 1",
            Ativo = true
        };
        _clientesApiController.ModelState.AddModelError("Nome", "Required");

        // Act
        var result = await _clientesApiController.AtualizarCliente(request.Id, request, CancellationToken.None);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        var response = Assert.IsType<BaseApiResponse>(badRequestResult.Value);
        Assert.False(response.Success);
        Assert.NotNull(response.Errors);
    }

    [Fact]
    public async Task AtualizarCliente_DeveRetornarBadRequest_QuandoIdNaoCorresponde()
    {
        // Arrange
        var request = new ClienteAtualizarRequestDto
        {
            Id = Guid.NewGuid(),
            Nome = "Cliente 1",
            Ativo = true
        };
        var idDiferente = Guid.NewGuid();

        // Act
        var result = await _clientesApiController.AtualizarCliente(idDiferente, request, CancellationToken.None);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        var response = Assert.IsType<BaseApiResponse>(badRequestResult.Value);
        Assert.False(response.Success);
        Assert.NotNull(response.Errors);
    }
}