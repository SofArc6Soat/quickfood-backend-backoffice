using Domain.Entities;
using Domain.Tests.TestHelpers;
using Gateways.Cognito;
using Infra.Dto;
using Infra.Repositories;
using Moq;

namespace Gateways.Tests;

public class ClienteGatewayTests
{
    private readonly Mock<IClienteRepository> _mockClienteRepository;
    private readonly Mock<IFuncionarioRepository> _mockFuncionarioRepository;
    private readonly Mock<ICognitoGateway> _mockCognitoGateway;
    private readonly ClienteGateway _clienteGateway;

    public ClienteGatewayTests()
    {
        _mockClienteRepository = new Mock<IClienteRepository>();
        _mockFuncionarioRepository = new Mock<IFuncionarioRepository>();
        _mockCognitoGateway = new Mock<ICognitoGateway>();
        _clienteGateway = new ClienteGateway(_mockClienteRepository.Object, _mockFuncionarioRepository.Object, _mockCognitoGateway.Object);
    }

    [Fact]
    public async Task CadastrarClienteAsync_DeveCadastrarClienteEEnviarEventoComSucesso()
    {
        // Arrange
        var cliente = ClienteFakeDataFactory.CriarClienteValido();
        var senha = "SenhaTeste123";
        var cancellationToken = CancellationToken.None;

        _mockClienteRepository.Setup(x => x.InsertAsync(It.IsAny<ClienteDb>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        _mockClienteRepository.Setup(x => x.UnitOfWork.CommitAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        _mockCognitoGateway.Setup(x => x.CriarUsuarioClienteAsync(It.IsAny<Cliente>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var resultado = await _clienteGateway.CadastrarClienteAsync(cliente, senha, cancellationToken);

        // Assert
        Assert.True(resultado);
        _mockClienteRepository.Verify(x => x.InsertAsync(It.IsAny<ClienteDb>(), It.IsAny<CancellationToken>()), Times.Once);
        _mockClienteRepository.Verify(x => x.UnitOfWork.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
        _mockCognitoGateway.Verify(x => x.CriarUsuarioClienteAsync(It.IsAny<Cliente>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task CadastrarClienteAsync_DeveRetornarFalso_QuandoCommitFalhar()
    {
        // Arrange
        var cliente = ClienteFakeDataFactory.CriarClienteValido();
        var senha = "SenhaTeste123";
        var cancellationToken = CancellationToken.None;

        _mockClienteRepository.Setup(x => x.InsertAsync(It.IsAny<ClienteDb>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        _mockClienteRepository.Setup(x => x.UnitOfWork.CommitAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        // Act
        var resultado = await _clienteGateway.CadastrarClienteAsync(cliente, senha, cancellationToken);

        // Assert
        Assert.False(resultado);
        _mockClienteRepository.Verify(x => x.InsertAsync(It.IsAny<ClienteDb>(), It.IsAny<CancellationToken>()), Times.Once);
        _mockClienteRepository.Verify(x => x.UnitOfWork.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
        _mockCognitoGateway.Verify(x => x.CriarUsuarioClienteAsync(It.IsAny<Cliente>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task CadastrarClienteAsync_DeveRetornarFalso_QuandoCriarUsuarioClienteFalhar()
    {
        // Arrange
        var cliente = ClienteFakeDataFactory.CriarClienteValido();
        var senha = "SenhaTeste123";
        var cancellationToken = CancellationToken.None;

        _mockClienteRepository.Setup(x => x.InsertAsync(It.IsAny<ClienteDb>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        _mockClienteRepository.Setup(x => x.UnitOfWork.CommitAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        _mockCognitoGateway.Setup(x => x.CriarUsuarioClienteAsync(It.IsAny<Cliente>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        // Act
        var resultado = await _clienteGateway.CadastrarClienteAsync(cliente, senha, cancellationToken);

        // Assert
        Assert.False(resultado);
        _mockClienteRepository.Verify(x => x.InsertAsync(It.IsAny<ClienteDb>(), It.IsAny<CancellationToken>()), Times.Once);
        _mockClienteRepository.Verify(x => x.UnitOfWork.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
        _mockCognitoGateway.Verify(x => x.CriarUsuarioClienteAsync(It.IsAny<Cliente>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task ObterTodosClientesAsync_DeveRetornarListaDeClientes()
    {
        // Arrange
        var clientesDb = new List<ClienteDb>
        {
            ClienteFakeDataFactory.CriarClienteDbValido(),
            new ClienteDb { Id = Guid.NewGuid(), Nome = "Cliente 2", Email = "cliente2@teste.com", Cpf = "12345678902", Ativo = true }
        };
        var cancellationToken = CancellationToken.None;

        _mockClienteRepository.Setup(x => x.ObterTodosClientesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(clientesDb);

        // Act
        var resultado = await _clienteGateway.ObterTodosClientesAsync(cancellationToken);

        // Assert
        Assert.NotNull(resultado);
        Assert.Equal(2, resultado.Count());
        _mockClienteRepository.Verify(x => x.ObterTodosClientesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}