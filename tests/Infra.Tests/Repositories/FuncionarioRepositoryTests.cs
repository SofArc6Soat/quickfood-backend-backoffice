using Core.Domain.Data;
using Domain.Tests.TestHelpers;
using Infra.Dto;
using Infra.Repositories;
using Moq;

namespace Infra.Tests.Repositories;

public class FuncionarioRepositoryTests
{
    private readonly Mock<IFuncionarioRepository> _mockFuncionarioRepository;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;

    public FuncionarioRepositoryTests()
    {
        _mockFuncionarioRepository = new Mock<IFuncionarioRepository>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockFuncionarioRepository.Setup(repo => repo.UnitOfWork).Returns(_mockUnitOfWork.Object);
    }

    [Fact]
    public async Task ObterTodosFuncionariosAsync_DeveRetornarListaDeFuncionarios()
    {
        // Arrange
        var funcionariosDb = new List<FuncionarioDb>
            {
                FuncionarioFakeDataFactory.CriarFuncionarioDbValido(),
                FuncionarioFakeDataFactory.CriarOutroFuncionarioDbValido()
            };
        var cancellationToken = CancellationToken.None;

        _mockFuncionarioRepository.Setup(x => x.FindAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(funcionariosDb);

        // Act
        var resultado = await _mockFuncionarioRepository.Object.FindAllAsync(cancellationToken);

        // Assert
        Assert.NotNull(resultado);
        Assert.Equal(2, resultado.Count);
        _mockFuncionarioRepository.Verify(x => x.FindAllAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task ObterTodosFuncionariosAsync_DeveLancarExcecao_QuandoOcorreErro()
    {
        // Arrange
        var cancellationToken = CancellationToken.None;

        _mockFuncionarioRepository.Setup(x => x.FindAllAsync(It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Erro ao obter todos os funcionários"));

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() => _mockFuncionarioRepository.Object.FindAllAsync(cancellationToken));
        Assert.Equal("Erro ao obter todos os funcionários", exception.Message);
        _mockFuncionarioRepository.Verify(x => x.FindAllAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task FindByIdAsync_DeveRetornarFuncionarioPorId()
    {
        // Arrange
        var funcionarioId = FuncionarioFakeDataFactory.ObterGuid();
        var funcionarioDb = FuncionarioFakeDataFactory.CriarFuncionarioDbValido();
        var cancellationToken = CancellationToken.None;

        _mockFuncionarioRepository.Setup(x => x.FindByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(funcionarioDb);

        // Act
        var resultado = await _mockFuncionarioRepository.Object.FindByIdAsync(funcionarioId, cancellationToken);

        // Assert
        Assert.NotNull(resultado);
        Assert.Equal(funcionarioId, resultado.Id);
        _mockFuncionarioRepository.Verify(x => x.FindByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task FindByIdAsync_DeveLancarExcecao_QuandoOcorreErro()
    {
        // Arrange
        var funcionarioId = FuncionarioFakeDataFactory.ObterGuid();
        var cancellationToken = CancellationToken.None;

        _mockFuncionarioRepository.Setup(x => x.FindByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Erro ao encontrar funcionário por ID"));

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() => _mockFuncionarioRepository.Object.FindByIdAsync(funcionarioId, cancellationToken));
        Assert.Equal("Erro ao encontrar funcionário por ID", exception.Message);
        _mockFuncionarioRepository.Verify(x => x.FindByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task InsertAsync_DeveInserirFuncionario()
    {
        // Arrange
        var funcionarioDb = FuncionarioFakeDataFactory.CriarFuncionarioDbValido();
        var cancellationToken = CancellationToken.None;

        _mockFuncionarioRepository.Setup(x => x.InsertAsync(It.IsAny<FuncionarioDb>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        await _mockFuncionarioRepository.Object.InsertAsync(funcionarioDb, cancellationToken);

        // Assert
        _mockFuncionarioRepository.Verify(x => x.InsertAsync(It.IsAny<FuncionarioDb>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task InsertAsync_DeveLancarExcecao_QuandoOcorreErro()
    {
        // Arrange
        var funcionarioDb = FuncionarioFakeDataFactory.CriarFuncionarioDbValido();
        var cancellationToken = CancellationToken.None;

        _mockFuncionarioRepository.Setup(x => x.InsertAsync(It.IsAny<FuncionarioDb>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Erro ao inserir funcionário"));

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() => _mockFuncionarioRepository.Object.InsertAsync(funcionarioDb, cancellationToken));
        Assert.Equal("Erro ao inserir funcionário", exception.Message);
        _mockFuncionarioRepository.Verify(x => x.InsertAsync(It.IsAny<FuncionarioDb>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_DeveDeletarFuncionario()
    {
        // Arrange
        var funcionarioDb = FuncionarioFakeDataFactory.CriarFuncionarioDbValido();
        var cancellationToken = CancellationToken.None;

        _mockFuncionarioRepository.Setup(x => x.DeleteAsync(It.IsAny<FuncionarioDb>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        await _mockFuncionarioRepository.Object.DeleteAsync(funcionarioDb, cancellationToken);

        // Assert
        _mockFuncionarioRepository.Verify(x => x.DeleteAsync(It.IsAny<FuncionarioDb>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_DeveLancarExcecao_QuandoOcorreErro()
    {
        // Arrange
        var funcionarioDb = FuncionarioFakeDataFactory.CriarFuncionarioDbValido();
        var cancellationToken = CancellationToken.None;

        _mockFuncionarioRepository.Setup(x => x.DeleteAsync(It.IsAny<FuncionarioDb>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Erro ao deletar funcionário"));

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() => _mockFuncionarioRepository.Object.DeleteAsync(funcionarioDb, cancellationToken));
        Assert.Equal("Erro ao deletar funcionário", exception.Message);
        _mockFuncionarioRepository.Verify(x => x.DeleteAsync(It.IsAny<FuncionarioDb>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}