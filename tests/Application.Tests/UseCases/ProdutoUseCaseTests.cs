using Core.Domain.Notificacoes;
using Domain.Entities;
using Domain.Tests.TestHelpers;
using Domain.ValueObjects;
using Gateways;
using Moq;
using UseCases;

namespace Application.Tests.UseCases
{
    public class ProdutoUseCaseTests
    {
        private readonly Mock<IProdutoGateway> _produtoGatewayMock;
        private readonly Mock<INotificador> _notificadorMock;
        private readonly IProdutoUseCase _produtoUseCase;

        public ProdutoUseCaseTests()
        {
            _produtoGatewayMock = new Mock<IProdutoGateway>();
            _notificadorMock = new Mock<INotificador>();
            _produtoUseCase = new ProdutoUseCase(_produtoGatewayMock.Object, _notificadorMock.Object);
        }

        [Fact]
        public async Task ObterTodosProdutosAsync_DeveRetornarListaDeProdutos()
        {
            // Arrange
            var produtos = ProdutoFakeDataFactory.CriarListaProdutosDbValidos().Select(p => new Produto(p.Id, p.Nome, p.Descricao, p.Preco, Enum.Parse<Categoria>(p.Categoria), p.Ativo)).ToList();

            _produtoGatewayMock.Setup(x => x.ObterTodosProdutosAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(produtos);

            // Act
            var result = await _produtoUseCase.ObterTodosProdutosAsync(CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task ObterProdutosCategoriaAsync_DeveRetornarListaDeProdutosDaCategoria()
        {
            // Arrange
            var categoria = Categoria.Lanche;
            var produtos = ProdutoFakeDataFactory.CriarListaProdutosDbValidos().Where(p => p.Categoria == categoria.ToString()).Select(p => new Produto(p.Id, p.Nome, p.Descricao, p.Preco, Enum.Parse<Categoria>(p.Categoria), p.Ativo)).ToList();

            _produtoGatewayMock.Setup(x => x.ObterProdutosCategoriaAsync(categoria, It.IsAny<CancellationToken>()))
                .ReturnsAsync(produtos);

            // Act
            var result = await _produtoUseCase.ObterProdutosCategoriaAsync(categoria, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Count());
        }

        [Fact]
        public async Task CadastrarProdutoAsync_DeveRetornarTrue_QuandoCadastroBemSucedido()
        {
            // Arrange
            var produto = ProdutoFakeDataFactory.CriarProdutoValido();

            _produtoGatewayMock.Setup(x => x.VerificarProdutoExistente(produto.Id, produto.Nome, produto.Descricao, It.IsAny<CancellationToken>()))
                .Returns(false);
            _produtoGatewayMock.Setup(x => x.CadastrarProdutoAsync(produto, It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            // Act
            var result = await _produtoUseCase.CadastrarProdutoAsync(produto, CancellationToken.None);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task CadastrarProdutoAsync_DeveRetornarFalse_QuandoProdutoJaExistente()
        {
            // Arrange
            var produto = ProdutoFakeDataFactory.CriarProdutoValido();

            _produtoGatewayMock.Setup(x => x.VerificarProdutoExistente(produto.Id, produto.Nome, produto.Descricao, It.IsAny<CancellationToken>()))
                .Returns(true);

            // Act
            var result = await _produtoUseCase.CadastrarProdutoAsync(produto, CancellationToken.None);

            // Assert
            Assert.False(result);
            _notificadorMock.Verify(x => x.Handle(It.IsAny<Notificacao>()), Times.Once);
        }

        [Fact]
        public async Task AtualizarProdutoAsync_DeveRetornarTrue_QuandoAtualizacaoBemSucedida()
        {
            // Arrange
            var produto = ProdutoFakeDataFactory.AlterarProdutoValido();

            _produtoGatewayMock.Setup(x => x.VerificarProdutoExistenteAsync(produto.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);
            _produtoGatewayMock.Setup(x => x.AtualizarProdutoAsync(produto, It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            // Act
            var result = await _produtoUseCase.AtualizarProdutoAsync(produto, CancellationToken.None);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task AtualizarProdutoAsync_DeveRetornarFalse_QuandoProdutoInexistente()
        {
            // Arrange
            var produto = ProdutoFakeDataFactory.AlterarProdutoValido();

            _produtoGatewayMock.Setup(x => x.VerificarProdutoExistenteAsync(produto.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            // Act
            var result = await _produtoUseCase.AtualizarProdutoAsync(produto, CancellationToken.None);

            // Assert
            Assert.False(result);
            _notificadorMock.Verify(x => x.Handle(It.IsAny<Notificacao>()), Times.Once);
        }

        [Fact]
        public async Task DeletarProdutoAsync_DeveRetornarTrue_QuandoDelecaoBemSucedida()
        {
            // Arrange
            var produtoId = ProdutoFakeDataFactory.ObterGuid();

            _produtoGatewayMock.Setup(x => x.VerificarProdutoExistenteAsync(produtoId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);
            _produtoGatewayMock.Setup(x => x.DeletarProdutoAsync(produtoId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            // Act
            var result = await _produtoUseCase.DeletarProdutoAsync(produtoId, CancellationToken.None);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task DeletarProdutoAsync_DeveRetornarFalse_QuandoProdutoInexistente()
        {
            // Arrange
            var produtoId = ProdutoFakeDataFactory.ObterGuidInvalido();

            _produtoGatewayMock.Setup(x => x.VerificarProdutoExistenteAsync(produtoId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            // Act
            var result = await _produtoUseCase.DeletarProdutoAsync(produtoId, CancellationToken.None);

            // Assert
            Assert.False(result);
            _notificadorMock.Verify(x => x.Handle(It.IsAny<Notificacao>()), Times.Once);
        }
    }
}