using Core.Domain.Notificacoes;
using Domain.Entities;
using Gateways;
using Moq;
using QuickFood_Backoffice.Tests.TestHelpers;
using UseCases;

namespace QuickFood_Backoffice.Tests.Domain.Entities.UseCases;

public class ClienteUseCaseTests
{
    private readonly Mock<IClienteGateway> _clienteGatewayMock;
    private readonly Mock<INotificador> _notificadorMock;
    private readonly IClienteUseCase _clienteUseCase;

    public ClienteUseCaseTests()
    {
        _clienteGatewayMock = new Mock<IClienteGateway>();
        _notificadorMock = new Mock<INotificador>();
        _clienteUseCase = new ClienteUseCase(_clienteGatewayMock.Object, _notificadorMock.Object);
    }

    [Fact]
    public async Task ObterTodosClientesAsync_DeveRetornarListaDeClientes()
    {
        // Arrange
        var clientes = new List<Cliente>
            {
                ClienteFakeDataFactory.CriarClienteValido(),
                new Cliente(ClienteFakeDataFactory.ObterOutroGuid(), "Cliente 2", "cliente2@example.com", "09876543210", true)
            };

        _clienteGatewayMock.Setup(x => x.ObterTodosClientesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(clientes);

        // Act
        var result = await _clienteUseCase.ObterTodosClientesAsync(CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task CadastrarClienteAsync_DeveRetornarTrue_QuandoCadastroBemSucedido()
    {
        // Arrange
        var cliente = ClienteFakeDataFactory.CriarClienteValido();
        var senha = "senha123";

        _clienteGatewayMock.Setup(x => x.VerificarClienteExistente(cliente.Id, cliente.Cpf, cliente.Email, It.IsAny<CancellationToken>()))
            .Returns(false);
        _clienteGatewayMock.Setup(x => x.CadastrarClienteAsync(cliente, senha, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var result = await _clienteUseCase.CadastrarClienteAsync(cliente, senha, CancellationToken.None);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task CadastrarClienteAsync_DeveRetornarFalse_QuandoClienteJaExistente()
    {
        // Arrange
        var cliente = ClienteFakeDataFactory.CriarClienteValido();
        var senha = "senha123";

        _clienteGatewayMock.Setup(x => x.VerificarClienteExistente(cliente.Id, cliente.Cpf, cliente.Email, It.IsAny<CancellationToken>()))
            .Returns(true);

        // Act
        var result = await _clienteUseCase.CadastrarClienteAsync(cliente, senha, CancellationToken.None);

        // Assert
        Assert.False(result);
        _notificadorMock.Verify(x => x.Handle(It.IsAny<Notificacao>()), Times.Once);
    }

    [Fact]
    public async Task CadastrarClienteAsync_DeveRetornarFalse_QuandoCadastroFalhar()
    {
        // Arrange
        var cliente = ClienteFakeDataFactory.CriarClienteValido();
        var senha = "senha123";

        _clienteGatewayMock.Setup(x => x.VerificarClienteExistente(cliente.Id, cliente.Cpf, cliente.Email, It.IsAny<CancellationToken>()))
            .Returns(false);
        _clienteGatewayMock.Setup(x => x.CadastrarClienteAsync(cliente, senha, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        // Act
        var result = await _clienteUseCase.CadastrarClienteAsync(cliente, senha, CancellationToken.None);

        // Assert
        Assert.False(result);
        _notificadorMock.Verify(x => x.Handle(It.IsAny<Notificacao>()), Times.Once);
    }

    [Fact]
    public async Task AtualizarClienteAsync_DeveRetornarTrue_QuandoAtualizacaoBemSucedida()
    {
        // Arrange
        var cliente = ClienteFakeDataFactory.CriarClienteValido();
        var clienteExistente = ClienteFakeDataFactory.CriarClienteValido();

        _clienteGatewayMock.Setup(x => x.VerificarClienteExistenteAsync(cliente.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(clienteExistente);
        _clienteGatewayMock.Setup(x => x.AtualizarClienteAsync(It.IsAny<Cliente>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var result = await _clienteUseCase.AtualizarClienteAsync(cliente, CancellationToken.None);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task AtualizarClienteAsync_DeveRetornarFalse_QuandoClienteInexistente()
    {
        // Arrange
        var cliente = ClienteFakeDataFactory.CriarClienteValido();

        _clienteGatewayMock.Setup(x => x.VerificarClienteExistenteAsync(cliente.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Cliente?)null);

        // Act
        var result = await _clienteUseCase.AtualizarClienteAsync(cliente, CancellationToken.None);

        // Assert
        Assert.False(result);
        _notificadorMock.Verify(x => x.Handle(It.IsAny<Notificacao>()), Times.Once);
    }

    [Fact]
    public async Task DeletarClienteAsync_DeveRetornarTrue_QuandoDelecaoBemSucedida()
    {
        // Arrange
        var clienteId = ClienteFakeDataFactory.ObterGuid();

        _clienteGatewayMock.Setup(x => x.VerificarClienteExistenteAsync(clienteId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(ClienteFakeDataFactory.CriarClienteValido());
        _clienteGatewayMock.Setup(x => x.DeletarClienteAsync(clienteId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var result = await _clienteUseCase.DeletarClienteAsync(clienteId, CancellationToken.None);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task DeletarClienteAsync_DeveRetornarFalse_QuandoClienteInexistente()
    {
        // Arrange
        var clienteId = ClienteFakeDataFactory.ObterGuidInvalido();

        _clienteGatewayMock.Setup(x => x.VerificarClienteExistenteAsync(clienteId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Cliente?)null);

        // Act
        var result = await _clienteUseCase.DeletarClienteAsync(clienteId, CancellationToken.None);

        // Assert
        Assert.False(result);
        _notificadorMock.Verify(x => x.Handle(It.IsAny<Notificacao>()), Times.Once);
    }
}