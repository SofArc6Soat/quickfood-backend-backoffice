using Domain.Entities;
using Domain.Tests.TestHelpers;
using Gateways.Cognito;
using Infra.Dto;
using Infra.Repositories;
using Moq;
using System.Linq.Expressions;

namespace Gateways.Tests.Gateways;

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
    public async Task AtualizarClienteAsync_DeveAtualizarClienteComSucesso()
    {
        // Arrange
        var cliente = ClienteFakeDataFactory.CriarClienteValido();
        var cancellationToken = CancellationToken.None;

        _mockClienteRepository.Setup(x => x.UpdateAsync(It.IsAny<ClienteDb>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        _mockClienteRepository.Setup(x => x.UnitOfWork.CommitAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var resultado = await _clienteGateway.AtualizarClienteAsync(cliente, cancellationToken);

        // Assert
        Assert.True(resultado);
        _mockClienteRepository.Verify(x => x.UpdateAsync(It.IsAny<ClienteDb>(), It.IsAny<CancellationToken>()), Times.Once);
        _mockClienteRepository.Verify(x => x.UnitOfWork.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task AtualizarClienteAsync_DeveRetornarFalso_QuandoCommitFalhar()
    {
        // Arrange
        var cliente = ClienteFakeDataFactory.CriarClienteValido();
        var cancellationToken = CancellationToken.None;

        _mockClienteRepository.Setup(x => x.UpdateAsync(It.IsAny<ClienteDb>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        _mockClienteRepository.Setup(x => x.UnitOfWork.CommitAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        // Act
        var resultado = await _clienteGateway.AtualizarClienteAsync(cliente, cancellationToken);

        // Assert
        Assert.False(resultado);
        _mockClienteRepository.Verify(x => x.UpdateAsync(It.IsAny<ClienteDb>(), It.IsAny<CancellationToken>()), Times.Once);
        _mockClienteRepository.Verify(x => x.UnitOfWork.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeletarClienteAsync_DeveDeletarClienteComSucesso()
    {
        // Arrange
        var clienteId = ClienteFakeDataFactory.CriarClienteDbValido().Id;
        var cancellationToken = CancellationToken.None;

        _mockClienteRepository.Setup(x => x.DeleteAsync(clienteId, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        _mockClienteRepository.Setup(x => x.UnitOfWork.CommitAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var resultado = await _clienteGateway.DeletarClienteAsync(clienteId, cancellationToken);

        // Assert
        Assert.True(resultado);
        _mockClienteRepository.Verify(x => x.DeleteAsync(clienteId, It.IsAny<CancellationToken>()), Times.Once);
        _mockClienteRepository.Verify(x => x.UnitOfWork.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeletarClienteAsync_DeveRetornarFalso_QuandoCommitFalhar()
    {
        // Arrange
        var clienteId = ClienteFakeDataFactory.CriarClienteDbValido().Id;
        var cancellationToken = CancellationToken.None;

        _mockClienteRepository.Setup(x => x.DeleteAsync(clienteId, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        _mockClienteRepository.Setup(x => x.UnitOfWork.CommitAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        // Act
        var resultado = await _clienteGateway.DeletarClienteAsync(clienteId, cancellationToken);

        // Assert
        Assert.False(resultado);
        _mockClienteRepository.Verify(x => x.DeleteAsync(clienteId, It.IsAny<CancellationToken>()), Times.Once);
        _mockClienteRepository.Verify(x => x.UnitOfWork.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public void VerificarClienteExistente_DeveRetornarTrue_QuandoClienteExistente()
    {
        // Arrange
        var clienteDb = ClienteFakeDataFactory.CriarClienteDbValido();
        var cancellationToken = CancellationToken.None;

        _mockClienteRepository
            .Setup(x => x.Find(It.Is<Expression<Func<ClienteDb, bool>>>(expr =>
                expr.Compile().Invoke(clienteDb)), It.IsAny<CancellationToken>()))
            .Returns(new List<ClienteDb> { clienteDb }.AsQueryable());

        _mockFuncionarioRepository
            .Setup(x => x.Find(It.IsAny<Expression<Func<FuncionarioDb, bool>>>(), It.IsAny<CancellationToken>()))
            .Returns(Enumerable.Empty<FuncionarioDb>().AsQueryable());

        // Act
        var result = _clienteGateway.VerificarClienteExistente(clienteDb.Id, clienteDb.Cpf, clienteDb.Email, cancellationToken);

        // Assert
        Assert.True(result);
        _mockClienteRepository.Verify(x => x.Find(It.Is<Expression<Func<ClienteDb, bool>>>(expr =>
            expr.Compile().Invoke(clienteDb)), It.IsAny<CancellationToken>()), Times.Once);
        _mockFuncionarioRepository.Verify(x => x.Find(It.IsAny<Expression<Func<FuncionarioDb, bool>>>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public void VerificarClienteExistente_DeveRetornarFalse_QuandoClienteNaoExistente()
    {
        // Arrange
        var clienteDb = ClienteFakeDataFactory.CriarClienteDbValido();
        var cancellationToken = CancellationToken.None;

        _mockClienteRepository
            .Setup(x => x.Find(It.IsAny<Expression<Func<ClienteDb, bool>>>(), It.IsAny<CancellationToken>()))
            .Returns(Enumerable.Empty<ClienteDb>().AsQueryable());

        _mockFuncionarioRepository
            .Setup(x => x.Find(It.IsAny<Expression<Func<FuncionarioDb, bool>>>(), It.IsAny<CancellationToken>()))
            .Returns(Enumerable.Empty<FuncionarioDb>().AsQueryable());

        // Act
        var result = _clienteGateway.VerificarClienteExistente(clienteDb.Id, clienteDb.Cpf, clienteDb.Email, cancellationToken);

        // Assert
        Assert.False(result);
        _mockClienteRepository.Verify(x => x.Find(It.IsAny<Expression<Func<ClienteDb, bool>>>(), It.IsAny<CancellationToken>()), Times.Once);
        _mockFuncionarioRepository.Verify(x => x.Find(It.IsAny<Expression<Func<FuncionarioDb, bool>>>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task VerificarClienteExistenteAsync_DeveRetornarCliente_QuandoClienteExistente()
    {
        // Arrange
        var clienteDb = ClienteFakeDataFactory.CriarClienteDbValido();
        var cancellationToken = CancellationToken.None;

        _mockClienteRepository.Setup(x => x.FindByIdAsync(clienteDb.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(clienteDb);

        _mockFuncionarioRepository
            .Setup(x => x.Find(It.IsAny<Expression<Func<FuncionarioDb, bool>>>(), It.IsAny<CancellationToken>()))
            .Returns(Enumerable.Empty<FuncionarioDb>().AsQueryable());

        // Act
        var result = await _clienteGateway.VerificarClienteExistenteAsync(clienteDb.Id, cancellationToken);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(clienteDb.Id, result?.Id);
        _mockClienteRepository.Verify(x => x.FindByIdAsync(clienteDb.Id, It.IsAny<CancellationToken>()), Times.Once);
        _mockFuncionarioRepository.Verify(x => x.Find(It.IsAny<Expression<Func<FuncionarioDb, bool>>>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task VerificarClienteExistenteAsync_DeveRetornarNull_QuandoClienteNaoExistente()
    {
        // Arrange
        var clienteDb = ClienteFakeDataFactory.CriarClienteDbValido();
        var cancellationToken = CancellationToken.None;

        _mockClienteRepository.Setup(x => x.FindByIdAsync(clienteDb.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((ClienteDb?)null);

        // Act
        var result = await _clienteGateway.VerificarClienteExistenteAsync(clienteDb.Id, cancellationToken);

        // Assert
        Assert.Null(result);
        _mockClienteRepository.Verify(x => x.FindByIdAsync(clienteDb.Id, It.IsAny<CancellationToken>()), Times.Once);
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

        _mockClienteRepository.Setup(x => x.ObterTodosClientesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(clientesDb);

        // Act
        var resultado = await _clienteGateway.ObterTodosClientesAsync(cancellationToken);

        // Assert
        Assert.NotNull(resultado);
        Assert.Equal(2, resultado.Count());
        _mockClienteRepository.Verify(x => x.ObterTodosClientesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task ObterTodosClientesAsync_DeveRetornarListaVazia_QuandoNaoExistemClientes()
    {
        // Arrange
        var cancellationToken = CancellationToken.None;

        _mockClienteRepository.Setup(x => x.ObterTodosClientesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<ClienteDb>());

        // Act
        var resultado = await _clienteGateway.ObterTodosClientesAsync(cancellationToken);

        // Assert
        Assert.NotNull(resultado);
        Assert.Empty(resultado);
        _mockClienteRepository.Verify(x => x.ObterTodosClientesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
