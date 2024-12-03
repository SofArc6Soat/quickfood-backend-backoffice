using Domain.Entities;
using Domain.ValueObjects;
using Gateways.Dtos.Request;
using Moq;
using UseCases;

namespace Controllers.Tests.Controllers;

public class ProdutoControllerTests
{
    private readonly Mock<IProdutoUseCase> _produtoUseCaseMock;
    private readonly IProdutoController _produtoController;

    public ProdutoControllerTests()
    {
        _produtoUseCaseMock = new Mock<IProdutoUseCase>();
        _produtoController = new ProdutoController(_produtoUseCaseMock.Object);
    }

    [Fact]
    public async Task CadastrarProdutoAsync_DeveRetornarTrue_QuandoProdutoValido()
    {
        // Arrange
        var produtoDto = new ProdutoRequestDto
        {
            Id = Guid.NewGuid(),
            Nome = "Produto Teste",
            Descricao = "Descrição do Produto Teste",
            Preco = 10.0m,
            Categoria = "Lanche",
            Ativo = true
        };
        _produtoUseCaseMock.Setup(x => x.CadastrarProdutoAsync(It.IsAny<Produto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var result = await _produtoController.CadastrarProdutoAsync(produtoDto, CancellationToken.None);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task CadastrarProdutoAsync_DeveRetornarFalse_QuandoCategoriaInvalida()
    {
        // Arrange
        var produtoDto = new ProdutoRequestDto
        {
            Id = Guid.NewGuid(),
            Nome = "Produto Teste",
            Descricao = "Descrição do Produto Teste",
            Preco = 10.0m,
            Categoria = "Invalida",
            Ativo = true
        };

        // Act
        var result = await _produtoController.CadastrarProdutoAsync(produtoDto, CancellationToken.None);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task AtualizarProdutoAsync_DeveRetornarTrue_QuandoProdutoValido()
    {
        // Arrange
        var produtoDto = new ProdutoRequestDto
        {
            Id = Guid.NewGuid(),
            Nome = "Produto Teste",
            Descricao = "Descrição do Produto Teste",
            Preco = 10.0m,
            Categoria = "Lanche",
            Ativo = true
        };
        _produtoUseCaseMock.Setup(x => x.AtualizarProdutoAsync(It.IsAny<Produto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var result = await _produtoController.AtualizarProdutoAsync(produtoDto, CancellationToken.None);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task AtualizarProdutoAsync_DeveRetornarFalse_QuandoCategoriaInvalida()
    {
        // Arrange
        var produtoDto = new ProdutoRequestDto
        {
            Id = Guid.NewGuid(),
            Nome = "Produto Teste",
            Descricao = "Descrição do Produto Teste",
            Preco = 10.0m,
            Categoria = "Invalida",
            Ativo = true
        };

        // Act
        var result = await _produtoController.AtualizarProdutoAsync(produtoDto, CancellationToken.None);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task DeletarProdutoAsync_DeveRetornarTrue_QuandoProdutoDeletado()
    {
        // Arrange
        var produtoId = Guid.NewGuid();
        _produtoUseCaseMock.Setup(x => x.DeletarProdutoAsync(produtoId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var result = await _produtoController.DeletarProdutoAsync(produtoId, CancellationToken.None);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task ObterTodosProdutosAsync_DeveRetornarListaDeProdutos()
    {
        // Arrange
        var produtos = new List<Produto>
            {
                new Produto(Guid.NewGuid(), "Produto 1", "Descrição 1", 10.0m, Categoria.Lanche, true),
                new Produto(Guid.NewGuid(), "Produto 2", "Descrição 2", 20.0m, Categoria.Bebida, true)
            };
        _produtoUseCaseMock.Setup(x => x.ObterTodosProdutosAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(produtos);

        // Act
        var result = await _produtoController.ObterTodosProdutosAsync(CancellationToken.None);

        // Assert
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task ObterProdutosCategoriaAsync_DeveRetornarListaDeProdutos_QuandoCategoriaValida()
    {
        // Arrange
        var produtos = new List<Produto>
            {
                new Produto(Guid.NewGuid(), "Produto 1", "Descrição 1", 10.0m, Categoria.Lanche, true),
                new Produto(Guid.NewGuid(), "Produto 2", "Descrição 2", 20.0m, Categoria.Lanche, true)
            };
        _produtoUseCaseMock.Setup(x => x.ObterProdutosCategoriaAsync(Categoria.Lanche, It.IsAny<CancellationToken>()))
            .ReturnsAsync(produtos);

        // Act
        var result = await _produtoController.ObterProdutosCategoriaAsync("Lanche", CancellationToken.None);

        // Assert
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task ObterProdutosCategoriaAsync_DeveRetornarListaVazia_QuandoCategoriaInvalida()
    {
        // Act
        var result = await _produtoController.ObterProdutosCategoriaAsync("Invalida", CancellationToken.None);

        // Assert
        Assert.Empty(result);
    }
}