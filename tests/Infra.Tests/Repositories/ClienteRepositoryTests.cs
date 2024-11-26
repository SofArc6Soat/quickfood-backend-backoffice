using Core.Domain.Data;
using Domain.Tests.TestHelpers;
using Infra.Dto;
using Infra.Repositories;
using Moq;

namespace Infra.Tests.Repositories;

public class ClienteRepositoryTests
{
    private readonly Mock<IClienteRepository> _mockClienteRepository;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;

    public ClienteRepositoryTests()
    {
        _mockClienteRepository = new Mock<IClienteRepository>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockClienteRepository.Setup(repo => repo.UnitOfWork).Returns(_mockUnitOfWork.Object);
    }

    [Fact]
    public async Task ObterTodosClientesAsync_DeveRetornarListaDeClientes()
    {
        // Arrange
        var clientesDb = new List<ClienteDb>
            {
                ClienteFakeDataFactory.CriarClienteDbValido(),
                ClienteFakeDataFactory.CriarOutroClienteDbValido()
            };
        var cancellationToken = CancellationToken.None;

        _mockClienteRepository.Setup(x => x.FindAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(clientesDb);

        // Act
        var resultado = await _mockClienteRepository.Object.FindAllAsync(cancellationToken);

        // Assert
        Assert.NotNull(resultado);
        Assert.Equal(2, resultado.Count);
        _mockClienteRepository.Verify(x => x.FindAllAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task ObterTodosClientesAsync_DeveLancarExcecao_QuandoOcorreErro()
    {
        // Arrange
        var cancellationToken = CancellationToken.None;

        _mockClienteRepository.Setup(x => x.FindAllAsync(It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Erro ao obter todos os clientes"));

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() => _mockClienteRepository.Object.FindAllAsync(cancellationToken));
        Assert.Equal("Erro ao obter todos os clientes", exception.Message);
        _mockClienteRepository.Verify(x => x.FindAllAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task FindByIdAsync_DeveRetornarClientePorId()
    {
        // Arrange
        var clienteId = ClienteFakeDataFactory.CriarClienteDbValido().Id;
        var clienteDb = ClienteFakeDataFactory.CriarClienteDbValido();
        var cancellationToken = CancellationToken.None;

        _mockClienteRepository.Setup(x => x.FindByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(clienteDb);

        // Act
        var resultado = await _mockClienteRepository.Object.FindByIdAsync(clienteId, cancellationToken);

        // Assert
        Assert.NotNull(resultado);
        Assert.Equal(clienteId, resultado.Id);
        _mockClienteRepository.Verify(x => x.FindByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task FindByIdAsync_DeveLancarExcecao_QuandoOcorreErro()
    {
        // Arrange
        var clienteId = ClienteFakeDataFactory.CriarClienteDbValido().Id;
        var cancellationToken = CancellationToken.None;

        _mockClienteRepository.Setup(x => x.FindByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Erro ao encontrar cliente por ID"));

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() => _mockClienteRepository.Object.FindByIdAsync(clienteId, cancellationToken));
        Assert.Equal("Erro ao encontrar cliente por ID", exception.Message);
        _mockClienteRepository.Verify(x => x.FindByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task InsertAsync_DeveInserirCliente()
    {
        // Arrange
        var clienteDb = ClienteFakeDataFactory.CriarClienteDbValido();
        var cancellationToken = CancellationToken.None;

        _mockClienteRepository.Setup(x => x.InsertAsync(It.IsAny<ClienteDb>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        await _mockClienteRepository.Object.InsertAsync(clienteDb, cancellationToken);

        // Assert
        _mockClienteRepository.Verify(x => x.InsertAsync(It.IsAny<ClienteDb>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task InsertAsync_DeveLancarExcecao_QuandoOcorreErro()
    {
        // Arrange
        var clienteDb = ClienteFakeDataFactory.CriarClienteDbValido();
        var cancellationToken = CancellationToken.None;

        _mockClienteRepository.Setup(x => x.InsertAsync(It.IsAny<ClienteDb>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Erro ao inserir cliente"));

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() => _mockClienteRepository.Object.InsertAsync(clienteDb, cancellationToken));
        Assert.Equal("Erro ao inserir cliente", exception.Message);
        _mockClienteRepository.Verify(x => x.InsertAsync(It.IsAny<ClienteDb>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_DeveDeletarCliente()
    {
        // Arrange
        var clienteDb = ClienteFakeDataFactory.CriarClienteDbValido();
        var cancellationToken = CancellationToken.None;

        _mockClienteRepository.Setup(x => x.DeleteAsync(It.IsAny<ClienteDb>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        await _mockClienteRepository.Object.DeleteAsync(clienteDb, cancellationToken);

        // Assert
        _mockClienteRepository.Verify(x => x.DeleteAsync(It.IsAny<ClienteDb>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_DeveLancarExcecao_QuandoOcorreErro()
    {
        // Arrange
        var clienteDb = ClienteFakeDataFactory.CriarClienteDbValido();
        var cancellationToken = CancellationToken.None;

        _mockClienteRepository.Setup(x => x.DeleteAsync(It.IsAny<ClienteDb>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Erro ao deletar cliente"));

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() => _mockClienteRepository.Object.DeleteAsync(clienteDb, cancellationToken));
        Assert.Equal("Erro ao deletar cliente", exception.Message);
        _mockClienteRepository.Verify(x => x.DeleteAsync(It.IsAny<ClienteDb>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}
