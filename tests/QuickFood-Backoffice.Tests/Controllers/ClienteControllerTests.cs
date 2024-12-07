using Controllers;
using Domain.Entities;
using Gateways.Dtos.Request;
using Moq;
using UseCases;

namespace QuickFood_Backoffice.Tests.Controllers;

public class ClienteControllerTests
{
    private readonly Mock<IClienteUseCase> _clienteUseCaseMock;
    private readonly IClienteController _clienteController;

    public ClienteControllerTests()
    {
        _clienteUseCaseMock = new Mock<IClienteUseCase>();
        _clienteController = new ClienteController(_clienteUseCaseMock.Object);
    }

    [Fact]
    public async Task CadastrarClienteAsync_DeveRetornarTrue_QuandoClienteValido()
    {
        // Arrange
        var clienteRequestDto = new ClienteRequestDto
        {
            Id = Guid.NewGuid(),
            Nome = "Cliente Teste",
            Email = "cliente@teste.com",
            Cpf = "12345678901",
            Senha = "senha123",
            Ativo = true
        };
        _clienteUseCaseMock.Setup(x => x.CadastrarClienteAsync(It.IsAny<Cliente>(), clienteRequestDto.Senha, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var result = await _clienteController.CadastrarClienteAsync(clienteRequestDto, CancellationToken.None);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task CadastrarClienteAsync_DeveRetornarFalse_QuandoCadastroFalhar()
    {
        // Arrange
        var clienteRequestDto = new ClienteRequestDto
        {
            Id = Guid.NewGuid(),
            Nome = "Cliente Teste",
            Email = "cliente@teste.com",
            Cpf = "12345678901",
            Senha = "senha123",
            Ativo = true
        };
        _clienteUseCaseMock.Setup(x => x.CadastrarClienteAsync(It.IsAny<Cliente>(), clienteRequestDto.Senha, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        // Act
        var result = await _clienteController.CadastrarClienteAsync(clienteRequestDto, CancellationToken.None);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task AtualizarClienteAsync_DeveRetornarTrue_QuandoClienteValido()
    {
        // Arrange
        var clienteAtualizarRequestDto = new ClienteAtualizarRequestDto
        {
            Id = Guid.NewGuid(),
            Nome = "Cliente Teste",
            Ativo = true
        };
        _clienteUseCaseMock.Setup(x => x.AtualizarClienteAsync(It.IsAny<Cliente>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var result = await _clienteController.AtualizarClienteAsync(clienteAtualizarRequestDto, CancellationToken.None);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task AtualizarClienteAsync_DeveRetornarFalse_QuandoAtualizacaoFalhar()
    {
        // Arrange
        var clienteAtualizarRequestDto = new ClienteAtualizarRequestDto
        {
            Id = Guid.NewGuid(),
            Nome = "Cliente Teste",
            Ativo = true
        };
        _clienteUseCaseMock.Setup(x => x.AtualizarClienteAsync(It.IsAny<Cliente>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        // Act
        var result = await _clienteController.AtualizarClienteAsync(clienteAtualizarRequestDto, CancellationToken.None);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task DeletarClienteAsync_DeveRetornarTrue_QuandoClienteDeletado()
    {
        // Arrange
        var clienteId = Guid.NewGuid();
        _clienteUseCaseMock.Setup(x => x.DeletarClienteAsync(clienteId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var result = await _clienteController.DeletarClienteAsync(clienteId, CancellationToken.None);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task ObterTodosClientesAsync_DeveRetornarListaDeClientes()
    {
        // Arrange
        var clientes = new List<Cliente>
            {
                new Cliente(Guid.NewGuid(), "Cliente 1", "cliente1@teste.com", "12345678901", true),
                new Cliente(Guid.NewGuid(), "Cliente 2", "cliente2@teste.com", "12345678902", true)
            };
        _clienteUseCaseMock.Setup(x => x.ObterTodosClientesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(clientes);

        // Act
        var result = await _clienteController.ObterTodosClientesAsync(CancellationToken.None);

        // Assert
        Assert.Equal(2, result.Count());
    }
}