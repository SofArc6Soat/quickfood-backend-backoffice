using Api.Controllers;
using Controllers;
using Core.Domain.Notificacoes;
using Core.WebApi.Controller;
using Domain.Entities;
using Domain.ValueObjects;
using Gateways.Dtos.Request;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace API.Tests.Controllers;

public class ProdutosApiControllerTests
{
    private readonly Mock<IProdutoController> _produtoControllerMock;
    private readonly Mock<INotificador> _notificadorMock;
    private readonly ProdutosApiController _produtosApiController;

    public ProdutosApiControllerTests()
    {
        _produtoControllerMock = new Mock<IProdutoController>();
        _notificadorMock = new Mock<INotificador>();
        _produtosApiController = new ProdutosApiController(_produtoControllerMock.Object, _notificadorMock.Object);
    }

    [Fact]
    public async Task ObterTodosProdutos_DeveRetornarOk_QuandoExistemProdutos()
    {
        // Arrange
        var produtos = new List<Produto> { new Produto(Guid.NewGuid(), "Produto 1", "Descrição 1", 10.0m, new Categoria(), true) };
        _produtoControllerMock.Setup(x => x.ObterTodosProdutosAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(produtos);

        // Act
        var result = await _produtosApiController.ObterTodosProdutos(CancellationToken.None);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var response = Assert.IsType<BaseApiResponse>(okResult.Value);
        Assert.True(response.Success);
        Assert.Equal(produtos, response.Data);
    }

    [Fact]
    public async Task ObterProdutosCategoria_DeveRetornarOk_QuandoExistemProdutosNaCategoria()
    {
        // Arrange
        var categoria = "Lanche";
        var produtos = new List<Produto> { new Produto(Guid.NewGuid(), "Produto 1", "Descrição 1", 10.0m, new Categoria(), true) };
        _produtoControllerMock.Setup(x => x.ObterProdutosCategoriaAsync(categoria, It.IsAny<CancellationToken>()))
            .ReturnsAsync(produtos);

        // Act
        var result = await _produtosApiController.ObterProdutosCategoria(categoria, CancellationToken.None);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var response = Assert.IsType<BaseApiResponse>(okResult.Value);
        Assert.True(response.Success);
        Assert.Equal(produtos, response.Data);
    }

    [Fact]
    public async Task CadastrarProduto_DeveRetornarCreated_QuandoProdutoValido()
    {
        // Arrange
        var request = new ProdutoRequestDto
        {
            Id = Guid.NewGuid(),
            Nome = "Produto 1",
            Descricao = "Descrição 1",
            Preco = 10.0m,
            Ativo = true,
            Categoria = "Lanche"
        };
        _produtoControllerMock.Setup(x => x.CadastrarProdutoAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var result = await _produtosApiController.CadastrarProduto(request, CancellationToken.None);

        // Assert
        var createdResult = Assert.IsType<CreatedResult>(result);
        var response = Assert.IsType<BaseApiResponse>(createdResult.Value);
        Assert.True(response.Success);
        Assert.True((bool)response.Data);
    }

    [Fact]
    public async Task CadastrarProduto_DeveRetornarBadRequest_QuandoModelStateInvalido()
    {
        // Arrange
        var request = new ProdutoRequestDto
        {
            Id = Guid.NewGuid(),
            Nome = "Produto 1",
            Descricao = "Descrição 1",
            Preco = 10.0m,
            Ativo = true,
            Categoria = "Lanche"
        };
        _produtosApiController.ModelState.AddModelError("Nome", "Required");

        // Act
        var result = await _produtosApiController.CadastrarProduto(request, CancellationToken.None);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        var response = Assert.IsType<BaseApiResponse>(badRequestResult.Value);
        Assert.False(response.Success);
        Assert.NotNull(response.Errors);
    }

    [Fact]
    public async Task AtualizarProduto_DeveRetornarOk_QuandoProdutoValido()
    {
        // Arrange
        var request = new ProdutoRequestDto
        {
            Id = Guid.NewGuid(),
            Nome = "Produto 1",
            Descricao = "Descrição 1",
            Preco = 10.0m,
            Ativo = true,
            Categoria = "Lanche"
        };
        _produtoControllerMock.Setup(x => x.AtualizarProdutoAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var result = await _produtosApiController.AtualizarProduto(request.Id, request, CancellationToken.None);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var response = Assert.IsType<BaseApiResponse>(okResult.Value);
        Assert.True(response.Success);
        Assert.True((bool)response.Data);
    }

    [Fact]
    public async Task AtualizarProduto_DeveRetornarBadRequest_QuandoModelStateInvalido()
    {
        // Arrange
        var request = new ProdutoRequestDto
        {
            Id = Guid.NewGuid(),
            Nome = "Produto 1",
            Descricao = "Descrição 1",
            Preco = 10.0m,
            Ativo = true,
            Categoria = "Lanche"
        };
        _produtosApiController.ModelState.AddModelError("Nome", "Required");

        // Act
        var result = await _produtosApiController.AtualizarProduto(request.Id, request, CancellationToken.None);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        var response = Assert.IsType<BaseApiResponse>(badRequestResult.Value);
        Assert.False(response.Success);
        Assert.NotNull(response.Errors);
    }

    [Fact]
    public async Task AtualizarProduto_DeveRetornarBadRequest_QuandoIdNaoCorresponde()
    {
        // Arrange
        var request = new ProdutoRequestDto
        {
            Id = Guid.NewGuid(),
            Nome = "Produto 1",
            Descricao = "Descrição 1",
            Preco = 10.0m,
            Ativo = true,
            Categoria = "Lanche"
        };
        var idDiferente = Guid.NewGuid();

        // Act
        var result = await _produtosApiController.AtualizarProduto(idDiferente, request, CancellationToken.None);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        var response = Assert.IsType<BaseApiResponse>(badRequestResult.Value);
        Assert.False(response.Success);
        Assert.NotNull(response.Errors);
    }

    [Fact]
    public async Task DeletarProduto_DeveRetornarNoContent_QuandoProdutoDeletado()
    {
        // Arrange
        var id = Guid.NewGuid();
        _produtoControllerMock.Setup(x => x.DeletarProdutoAsync(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var result = await _produtosApiController.DeletarProduto(id, CancellationToken.None);

        // Assert
        var noContentResult = Assert.IsType<NoContentResult>(result);
    }
}