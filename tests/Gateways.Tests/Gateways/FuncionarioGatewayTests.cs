using Domain.Entities;
using Domain.Tests.TestHelpers;
using Gateways.Cognito;
using Infra.Dto;
using Infra.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Gateways.Tests.Gateways;

public class FuncionarioGatewayTests
{
    private readonly Mock<IFuncionarioRepository> _mockFuncionarioRepository;
    private readonly Mock<IClienteRepository> _mockClienteRepository;
    private readonly Mock<ICognitoGateway> _mockCognitoGateway;
    private readonly FuncionarioGateway _funcionarioGateway;

    public FuncionarioGatewayTests()
    {
        _mockFuncionarioRepository = new Mock<IFuncionarioRepository>();
        _mockClienteRepository = new Mock<IClienteRepository>();
        _mockCognitoGateway = new Mock<ICognitoGateway>();
        _funcionarioGateway = new FuncionarioGateway(_mockFuncionarioRepository.Object, _mockClienteRepository.Object, _mockCognitoGateway.Object);
    }

    [Fact]
    public async Task CadastrarFuncionarioAsync_DeveCadastrarFuncionarioEEnviarEventoComSucesso()
    {
        // Arrange
        var funcionario = new Funcionario(FuncionarioFakeDataFactory.ObterGuid(), "Funcionario Exemplo", "funcionario@exemplo.com", true);
        var senha = "SenhaTeste123";
        var cancellationToken = CancellationToken.None;

        _mockFuncionarioRepository.Setup(x => x.InsertAsync(It.IsAny<FuncionarioDb>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        _mockFuncionarioRepository.Setup(x => x.UnitOfWork.CommitAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        _mockCognitoGateway.Setup(x => x.CriarUsuarioFuncionarioAsync(It.IsAny<Funcionario>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var resultado = await _funcionarioGateway.CadastrarFuncionarioAsync(funcionario, senha, cancellationToken);

        // Assert
        Assert.True(resultado);
        _mockFuncionarioRepository.Verify(x => x.InsertAsync(It.IsAny<FuncionarioDb>(), It.IsAny<CancellationToken>()), Times.Once);
        _mockFuncionarioRepository.Verify(x => x.UnitOfWork.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
        _mockCognitoGateway.Verify(x => x.CriarUsuarioFuncionarioAsync(It.IsAny<Funcionario>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task CadastrarFuncionarioAsync_DeveRetornarFalso_QuandoCommitFalhar()
    {
        // Arrange
        var funcionario = new Funcionario(FuncionarioFakeDataFactory.ObterGuid(), "Funcionario Exemplo", "funcionario@exemplo.com", true);
        var senha = "SenhaTeste123";
        var cancellationToken = CancellationToken.None;

        _mockFuncionarioRepository.Setup(x => x.InsertAsync(It.IsAny<FuncionarioDb>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        _mockFuncionarioRepository.Setup(x => x.UnitOfWork.CommitAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        // Act
        var resultado = await _funcionarioGateway.CadastrarFuncionarioAsync(funcionario, senha, cancellationToken);

        // Assert
        Assert.False(resultado);
        _mockFuncionarioRepository.Verify(x => x.InsertAsync(It.IsAny<FuncionarioDb>(), It.IsAny<CancellationToken>()), Times.Once);
        _mockFuncionarioRepository.Verify(x => x.UnitOfWork.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
        _mockCognitoGateway.Verify(x => x.CriarUsuarioFuncionarioAsync(It.IsAny<Funcionario>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task CadastrarFuncionarioAsync_DeveRetornarFalso_QuandoCriarUsuarioFuncionarioFalhar()
    {
        // Arrange
        var funcionario = new Funcionario(FuncionarioFakeDataFactory.ObterGuid(), "Funcionario Exemplo", "funcionario@exemplo.com", true);
        var senha = "SenhaTeste123";
        var cancellationToken = CancellationToken.None;

        _mockFuncionarioRepository.Setup(x => x.InsertAsync(It.IsAny<FuncionarioDb>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        _mockFuncionarioRepository.Setup(x => x.UnitOfWork.CommitAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        _mockCognitoGateway.Setup(x => x.CriarUsuarioFuncionarioAsync(It.IsAny<Funcionario>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        // Act
        var resultado = await _funcionarioGateway.CadastrarFuncionarioAsync(funcionario, senha, cancellationToken);

        // Assert
        Assert.False(resultado);
        _mockFuncionarioRepository.Verify(x => x.InsertAsync(It.IsAny<FuncionarioDb>(), It.IsAny<CancellationToken>()), Times.Once);
        _mockFuncionarioRepository.Verify(x => x.UnitOfWork.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
        _mockCognitoGateway.Verify(x => x.CriarUsuarioFuncionarioAsync(It.IsAny<Funcionario>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public void VerificarFuncionarioExistente_DeveRetornarTrue_QuandoFuncionarioExistente()
    {
        // Arrange
        var funcionarioDb = FuncionarioFakeDataFactory.CriarFuncionarioDbValido();
        var cancellationToken = CancellationToken.None;

        _mockFuncionarioRepository
            .Setup(x => x.Find(It.Is<Expression<Func<FuncionarioDb, bool>>>(expr =>
                expr.Compile().Invoke(funcionarioDb)), It.IsAny<CancellationToken>()))
            .Returns(new List<FuncionarioDb> { funcionarioDb }.AsQueryable());

        _mockClienteRepository
            .Setup(x => x.Find(It.IsAny<Expression<Func<ClienteDb, bool>>>(), It.IsAny<CancellationToken>()))
            .Returns(Enumerable.Empty<ClienteDb>().AsQueryable());

        // Act
        var result = _funcionarioGateway.VerificarFuncionarioExistente(funcionarioDb.Id, funcionarioDb.Email, cancellationToken);

        // Assert
        Assert.True(result);
        _mockFuncionarioRepository.Verify(x => x.Find(It.Is<Expression<Func<FuncionarioDb, bool>>>(expr =>
            expr.Compile().Invoke(funcionarioDb)), It.IsAny<CancellationToken>()), Times.Once);
        _mockClienteRepository.Verify(x => x.Find(It.IsAny<Expression<Func<ClienteDb, bool>>>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public void VerificarFuncionarioExistente_DeveRetornarFalse_QuandoFuncionarioNaoExistente()
    {
        // Arrange
        var funcionarioDb = FuncionarioFakeDataFactory.CriarFuncionarioDbValido();
        var cancellationToken = CancellationToken.None;

        _mockFuncionarioRepository
            .Setup(x => x.Find(It.IsAny<Expression<Func<FuncionarioDb, bool>>>(), It.IsAny<CancellationToken>()))
            .Returns(Enumerable.Empty<FuncionarioDb>().AsQueryable());

        _mockClienteRepository
            .Setup(x => x.Find(It.IsAny<Expression<Func<ClienteDb, bool>>>(), It.IsAny<CancellationToken>()))
            .Returns(Enumerable.Empty<ClienteDb>().AsQueryable());

        // Act
        var result = _funcionarioGateway.VerificarFuncionarioExistente(funcionarioDb.Id, funcionarioDb.Email, cancellationToken);

        // Assert
        Assert.False(result);
        _mockFuncionarioRepository.Verify(x => x.Find(It.IsAny<Expression<Func<FuncionarioDb, bool>>>(), It.IsAny<CancellationToken>()), Times.Once);
        _mockClienteRepository.Verify(x => x.Find(It.IsAny<Expression<Func<ClienteDb, bool>>>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}
