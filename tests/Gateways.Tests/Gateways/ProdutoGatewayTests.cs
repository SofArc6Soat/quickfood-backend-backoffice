using Core.Infra.MessageBroker;
using Domain.Tests.TestHelpers;
using Domain.ValueObjects;
using Gateways.Dtos.Events;
using Infra.Dto;
using Infra.Repositories;
using Moq;
using System.Linq.Expressions;

namespace Gateways.Tests.Gateways;

public class ProdutoGatewayTests
{
    private readonly Mock<IProdutoRepository> _produtoRepositoryMock;
    private readonly Mock<ISqsService<ProdutoCriadoEvent>> _sqsProdutoCriadoMock;
    private readonly Mock<ISqsService<ProdutoAtualizadoEvent>> _sqsProdutoAtualizadoMock;
    private readonly Mock<ISqsService<ProdutoExcluidoEvent>> _sqsProdutoExcluidoMock;
    private readonly ProdutoGateway _produtoGateway;

    public ProdutoGatewayTests()
    {
        _produtoRepositoryMock = new Mock<IProdutoRepository>();
        _sqsProdutoCriadoMock = new Mock<ISqsService<ProdutoCriadoEvent>>();
        _sqsProdutoAtualizadoMock = new Mock<ISqsService<ProdutoAtualizadoEvent>>();
        _sqsProdutoExcluidoMock = new Mock<ISqsService<ProdutoExcluidoEvent>>();

        _produtoGateway = new ProdutoGateway(
            _produtoRepositoryMock.Object,
            _sqsProdutoCriadoMock.Object,
            _sqsProdutoAtualizadoMock.Object,
            _sqsProdutoExcluidoMock.Object);
    }

    [Fact]
    public async Task CadastrarProdutoAsync_DeveCadastrarProdutoEEnviarEventoComSucesso()
    {
        // Arrange
        var produto = ProdutoFakeDataFactory.CriarProdutoValido();

        _produtoRepositoryMock.Setup(x => x.InsertAsync(It.IsAny<ProdutoDb>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        _produtoRepositoryMock.Setup(x => x.UnitOfWork.CommitAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        _sqsProdutoCriadoMock.Setup(x => x.SendMessageAsync(It.IsAny<ProdutoCriadoEvent>()))
            .ReturnsAsync(true);

        // Act
        var result = await _produtoGateway.CadastrarProdutoAsync(produto, CancellationToken.None);

        // Assert
        Assert.True(result);

        _produtoRepositoryMock.Verify(x => x.InsertAsync(It.IsAny<ProdutoDb>(), It.IsAny<CancellationToken>()), Times.Once);
        _produtoRepositoryMock.Verify(x => x.UnitOfWork.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);

        _sqsProdutoCriadoMock.Verify(x => x.SendMessageAsync(It.IsAny<ProdutoCriadoEvent>()), Times.Once);
    }

    [Fact]
    public async Task CadastrarProdutoAsync_DeveRetornarMensagemErro_QuandoFalharAoInserirProduto()
    {
        // Arrange
        var produto = ProdutoFakeDataFactory.CriarProdutoInvalido();
        var mensagemErroEsperada = "Erro ao inserir produto no banco de dados";

        _produtoRepositoryMock.Setup(x => x.InsertAsync(It.IsAny<ProdutoDb>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception(mensagemErroEsperada));

        // Act
        var exception = await Assert.ThrowsAsync<Exception>(() =>
            _produtoGateway.CadastrarProdutoAsync(produto, CancellationToken.None));

        // Assert
        Assert.NotNull(exception);
        Assert.Equal(mensagemErroEsperada, exception.Message);

        _produtoRepositoryMock.Verify(x => x.InsertAsync(It.IsAny<ProdutoDb>(), It.IsAny<CancellationToken>()), Times.Once);
        _produtoRepositoryMock.Verify(x => x.UnitOfWork.CommitAsync(It.IsAny<CancellationToken>()), Times.Never);

        _sqsProdutoCriadoMock.Verify(x => x.SendMessageAsync(It.IsAny<ProdutoCriadoEvent>()), Times.Never);
    }

    [Fact]
    public async Task AtualizarProdutoAsync_DeveAtualizarProdutoComSucesso()
    {
        // Arrange
        var produto = ProdutoFakeDataFactory.CriarProdutoValido();

        _produtoRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<ProdutoDb>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        _produtoRepositoryMock.Setup(x => x.UnitOfWork.CommitAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        _sqsProdutoAtualizadoMock.Setup(x => x.SendMessageAsync(It.IsAny<ProdutoAtualizadoEvent>()))
            .ReturnsAsync(true);

        // Act
        var result = await _produtoGateway.AtualizarProdutoAsync(produto, CancellationToken.None);

        // Assert
        Assert.True(result);

        _produtoRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<ProdutoDb>(), It.IsAny<CancellationToken>()), Times.Once);
        _produtoRepositoryMock.Verify(x => x.UnitOfWork.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
        _sqsProdutoAtualizadoMock.Verify(x => x.SendMessageAsync(It.IsAny<ProdutoAtualizadoEvent>()), Times.Once);
    }

    [Fact]
    public async Task AtualizarProdutoAsync_DeveRetornarMensagemErro_QuandoFalharAoAtualizarProduto()
    {
        // Arrange
        var produto = ProdutoFakeDataFactory.CriarProdutoValido();
        var mensagemErroEsperada = "Erro ao atualizar produto no banco de dados";

        _produtoRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<ProdutoDb>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception(mensagemErroEsperada));

        // Act
        var exception = await Assert.ThrowsAsync<Exception>(() =>
            _produtoGateway.AtualizarProdutoAsync(produto, CancellationToken.None));

        // Assert
        Assert.NotNull(exception);
        Assert.Equal(mensagemErroEsperada, exception.Message);

        _produtoRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<ProdutoDb>(), It.IsAny<CancellationToken>()), Times.Once);
        _produtoRepositoryMock.Verify(x => x.UnitOfWork.CommitAsync(It.IsAny<CancellationToken>()), Times.Never);

        _sqsProdutoAtualizadoMock.Verify(x => x.SendMessageAsync(It.IsAny<ProdutoAtualizadoEvent>()), Times.Never);
    }

    [Fact]
    public async Task DeletarProdutoAsync_DeveDeletarProdutoComSucesso()
    {
        // Arrange
        var produtoId = ProdutoFakeDataFactory.ObterGuid();

        _produtoRepositoryMock.Setup(x => x.DeleteAsync(produtoId, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        _produtoRepositoryMock.Setup(x => x.UnitOfWork.CommitAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        _sqsProdutoExcluidoMock.Setup(x => x.SendMessageAsync(It.IsAny<ProdutoExcluidoEvent>()))
            .ReturnsAsync(true);

        // Act
        var result = await _produtoGateway.DeletarProdutoAsync(produtoId, CancellationToken.None);

        // Assert
        Assert.True(result);

        _produtoRepositoryMock.Verify(x => x.DeleteAsync(produtoId, It.IsAny<CancellationToken>()), Times.Once);
        _produtoRepositoryMock.Verify(x => x.UnitOfWork.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
        _sqsProdutoExcluidoMock.Verify(x => x.SendMessageAsync(It.IsAny<ProdutoExcluidoEvent>()), Times.Once);
    }

    [Fact]
    public async Task DeletarProdutoAsync_DeveRetornarMensagemErro_QuandoFalharAoDeletarProduto()
    {
        // Arrange
        var produtoId = ProdutoFakeDataFactory.ObterGuid();
        var mensagemErroEsperada = "Erro ao deletar produto no banco de dados";

        _produtoRepositoryMock.Setup(x => x.DeleteAsync(produtoId, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception(mensagemErroEsperada));

        // Act
        var exception = await Assert.ThrowsAsync<Exception>(() =>
            _produtoGateway.DeletarProdutoAsync(produtoId, CancellationToken.None));

        // Assert
        Assert.NotNull(exception);
        Assert.Equal(mensagemErroEsperada, exception.Message);

        _produtoRepositoryMock.Verify(x => x.DeleteAsync(produtoId, It.IsAny<CancellationToken>()), Times.Once);
        _produtoRepositoryMock.Verify(x => x.UnitOfWork.CommitAsync(It.IsAny<CancellationToken>()), Times.Never);

        _sqsProdutoExcluidoMock.Verify(x => x.SendMessageAsync(It.IsAny<ProdutoExcluidoEvent>()), Times.Never);
    }

    [Fact]
    public void VerificarProdutoExistente_DeveRetornarTrue_QuandoProdutoExistente()
    {
        // Arrange
        var produtoDb = ProdutoFakeDataFactory.CriarProdutoDbValido();

        _produtoRepositoryMock
            .Setup(x => x.Find(It.Is<Expression<Func<ProdutoDb, bool>>>(expr =>
                expr.Compile().Invoke(produtoDb)), It.IsAny<CancellationToken>()))
            .Returns(new List<ProdutoDb> { produtoDb }.AsQueryable());

        // Act
        var result = _produtoGateway.VerificarProdutoExistente(produtoDb.Id, produtoDb.Nome, produtoDb.Descricao, CancellationToken.None);

        // Assert
        Assert.True(result);

        _produtoRepositoryMock.Verify(x => x.Find(It.Is<Expression<Func<ProdutoDb, bool>>>(expr =>
            expr.Compile().Invoke(produtoDb)), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public void VerificarProdutoExistente_DeveRetornarFalse_QuandoProdutoNaoExistente()
    {
        // Arrange
        var produtoDb = ProdutoFakeDataFactory.CriarProdutoDbValido();

        _produtoRepositoryMock
            .Setup(x => x.Find(It.IsAny<Expression<Func<ProdutoDb, bool>>>(), It.IsAny<CancellationToken>()))
            .Returns(Enumerable.Empty<ProdutoDb>().AsQueryable());

        // Act
        var result = _produtoGateway.VerificarProdutoExistente(produtoDb.Id, produtoDb.Nome, produtoDb.Descricao, CancellationToken.None);

        // Assert
        Assert.False(result, "O método VerificarProdutoExistente deveria retornar false quando o produto não existe.");

        _produtoRepositoryMock.Verify(x => x.Find(It.IsAny<Expression<Func<ProdutoDb, bool>>>(), It.IsAny<CancellationToken>()), Times.Once);
    }
    [Fact]
    public async Task VerificarProdutoExistenteAsync_DeveRetornarTrue_QuandoProdutoExistente()
    {
        // Arrange
        var produtoDb = ProdutoFakeDataFactory.CriarProdutoDbValido();

        _produtoRepositoryMock
            .Setup(x => x.FindByIdAsync(produtoDb.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(produtoDb);

        // Act
        var result = await _produtoGateway.VerificarProdutoExistenteAsync(produtoDb.Id, CancellationToken.None);

        // Assert
        Assert.True(result);

        _produtoRepositoryMock.Verify(x => x.FindByIdAsync(produtoDb.Id, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task VerificarProdutoExistenteAsync_DeveRetornarFalse_QuandoProdutoNaoExistente()
    {
        // Arrange
        var produtoId = ProdutoFakeDataFactory.ObterGuid();

        _produtoRepositoryMock
            .Setup(x => x.FindByIdAsync(produtoId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((ProdutoDb?)null);

        // Act
        var result = await _produtoGateway.VerificarProdutoExistenteAsync(produtoId, CancellationToken.None);

        // Assert
        Assert.False(result);

        _produtoRepositoryMock.Verify(x => x.FindByIdAsync(produtoId, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task ObterProdutoAsync_DeveRetornarProduto_QuandoProdutoExistente()
    {
        // Arrange
        var produtoDb = ProdutoFakeDataFactory.CriarProdutoDbValido();

        _produtoRepositoryMock
            .Setup(x => x.FindByIdAsync(produtoDb.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(produtoDb);

        // Act
        var result = await _produtoGateway.ObterProdutoAsync(produtoDb.Id, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(produtoDb.Id, result?.Id);

        _produtoRepositoryMock.Verify(x => x.FindByIdAsync(produtoDb.Id, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task ObterProdutoAsync_DeveRetornarNull_QuandoProdutoNaoExistente()
    {
        // Arrange
        var produtoId = ProdutoFakeDataFactory.ObterGuid();

        _produtoRepositoryMock
            .Setup(x => x.FindByIdAsync(produtoId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((ProdutoDb?)null);

        // Act
        var result = await _produtoGateway.ObterProdutoAsync(produtoId, CancellationToken.None);

        // Assert
        Assert.Null(result);

        _produtoRepositoryMock.Verify(x => x.FindByIdAsync(produtoId, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task ObterTodosProdutosAsync_DeveRetornarListaDeProdutos_QuandoProdutosExistem()
    {
        // Arrange
        var produtosDb = ProdutoFakeDataFactory.CriarListaProdutosDbValidos();

        _produtoRepositoryMock
            .Setup(x => x.ObterTodosProdutosAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(produtosDb);

        // Act
        var result = await _produtoGateway.ObterTodosProdutosAsync(CancellationToken.None);

        // Assert
        Assert.NotEmpty(result);
        Assert.Equal(produtosDb.Count(), result.Count());

        _produtoRepositoryMock.Verify(x => x.ObterTodosProdutosAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task ObterTodosProdutosAsync_DeveRetornarListaVazia_QuandoNenhumProdutoExistir()
    {
        // Arrange
        _produtoRepositoryMock
            .Setup(x => x.ObterTodosProdutosAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<ProdutoDb>());

        // Act
        var result = await _produtoGateway.ObterTodosProdutosAsync(CancellationToken.None);

        // Assert
        Assert.Empty(result);

        _produtoRepositoryMock.Verify(x => x.ObterTodosProdutosAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task ObterProdutosCategoriaAsync_DeveRetornarListaDeProdutos_QuandoProdutosExistemNaCategoria()
    {
        // Arrange
        var categoria = Categoria.Lanche;
        var produtosDb = ProdutoFakeDataFactory.CriarListaProdutosDbValidos();

        _produtoRepositoryMock
            .Setup(x => x.ObterProdutosCategoriaAsync(categoria.ToString(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(produtosDb);

        // Act
        var result = await _produtoGateway.ObterProdutosCategoriaAsync(categoria, CancellationToken.None);

        // Assert
        Assert.NotEmpty(result);
        Assert.Equal(produtosDb.Count(), result.Count());

        _produtoRepositoryMock.Verify(x => x.ObterProdutosCategoriaAsync(categoria.ToString(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task ObterProdutosCategoriaAsync_DeveRetornarListaVazia_QuandoNenhumProdutoExistirNaCategoria()
    {
        // Arrange
        var categoria = Categoria.Lanche;

        _produtoRepositoryMock
            .Setup(x => x.ObterProdutosCategoriaAsync(categoria.ToString(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<ProdutoDb>());

        // Act
        var result = await _produtoGateway.ObterProdutosCategoriaAsync(categoria, CancellationToken.None);

        // Assert
        Assert.Empty(result);

        _produtoRepositoryMock.Verify(x => x.ObterProdutosCategoriaAsync(categoria.ToString(), It.IsAny<CancellationToken>()), Times.Once);
    }
}
