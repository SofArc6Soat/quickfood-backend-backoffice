using Domain.Entities;
using Domain.ValueObjects;
using Infra.Dto;
using Infra.Repositories;
using Moq;
using System.Linq.Expressions;

namespace Gateways.Tests
{
    public class ProdutoGatewayTests
    {
        private readonly Mock<IProdutoRepository> _produtoRepositoryMock;
        private readonly ProdutoGateway _produtoGateway;

        public ProdutoGatewayTests()
        {
            _produtoRepositoryMock = new Mock<IProdutoRepository>();
            _produtoGateway = new ProdutoGateway(_produtoRepositoryMock.Object);
        }

        [Fact]
        public async Task CadastrarProdutoAsync_DeveCadastrarProdutoComSucesso()
        {
            // Arrange
            var produto = new Produto(Guid.NewGuid(), "Produto Teste", "Descrição Teste", 100.00m, Categoria.Lanche, true);
            _produtoRepositoryMock.Setup(x => x.InsertAsync(It.IsAny<ProdutoDb>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);
            _produtoRepositoryMock.Setup(x => x.UnitOfWork.CommitAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            // Act
            var result = await _produtoGateway.CadastrarProdutoAsync(produto, CancellationToken.None);

            // Assert
            Assert.True(result);
            _produtoRepositoryMock.Verify(x => x.InsertAsync(It.IsAny<ProdutoDb>(), It.IsAny<CancellationToken>()), Times.Once);
            _produtoRepositoryMock.Verify(x => x.UnitOfWork.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task AtualizarProdutoAsync_DeveAtualizarProdutoComSucesso()
        {
            // Arrange
            var produto = new Produto(Guid.NewGuid(), "Produto Teste", "Descrição Teste", 100.00m, Categoria.Lanche, true);
            _produtoRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<ProdutoDb>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);
            _produtoRepositoryMock.Setup(x => x.UnitOfWork.CommitAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            // Act
            var result = await _produtoGateway.AtualizarProdutoAsync(produto, CancellationToken.None);

            // Assert
            Assert.True(result);
            _produtoRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<ProdutoDb>(), It.IsAny<CancellationToken>()), Times.Once);
            _produtoRepositoryMock.Verify(x => x.UnitOfWork.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DeletarProdutoAsync_DeveDeletarProdutoComSucesso()
        {
            // Arrange
            var produtoId = Guid.NewGuid();
            _produtoRepositoryMock.Setup(x => x.DeleteAsync(produtoId, It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);
            _produtoRepositoryMock.Setup(x => x.UnitOfWork.CommitAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            // Act
            var result = await _produtoGateway.DeletarProdutoAsync(produtoId, CancellationToken.None);

            // Assert
            Assert.True(result);
            _produtoRepositoryMock.Verify(x => x.DeleteAsync(produtoId, It.IsAny<CancellationToken>()), Times.Once);
            _produtoRepositoryMock.Verify(x => x.UnitOfWork.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public void VerificarProdutoExistente_DeveRetornarTrue_QuandoProdutoExistente()
        {
            // Arrange
            var produtoId = Guid.NewGuid();
            var produtoDb = new ProdutoDb { Id = produtoId, Nome = "Nome", Descricao = "Descrição" };
            _produtoRepositoryMock
                .Setup(x => x.Find(It.Is<Expression<Func<ProdutoDb, bool>>>(expr =>
                    expr.Compile().Invoke(produtoDb)), It.IsAny<CancellationToken>()))
                .Returns(new List<ProdutoDb> { produtoDb }.AsQueryable());

            // Act
            var result = _produtoGateway.VerificarProdutoExistente(produtoId, "Nome", "Descrição", CancellationToken.None);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task VerificarProdutoExistenteAsync_DeveRetornarTrue_QuandoProdutoExistente()
        {
            // Arrange
            var produtoId = Guid.NewGuid();
            _produtoRepositoryMock.Setup(x => x.FindByIdAsync(produtoId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ProdutoDb { Id = produtoId });

            // Act
            var result = await _produtoGateway.VerificarProdutoExistenteAsync(produtoId, CancellationToken.None);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task ObterProdutoAsync_DeveRetornarProduto_QuandoProdutoExistente()
        {
            // Arrange
            var produtoId = Guid.NewGuid();
            var produtoDb = new ProdutoDb
            {
                Id = produtoId,
                Nome = "Produto Teste",
                Descricao = "Descrição Teste",
                Preco = 100.00m,
                Categoria = Categoria.Lanche.ToString(),
                Ativo = true
            };
            _produtoRepositoryMock.Setup(x => x.FindByIdAsync(produtoId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(produtoDb);

            // Act
            var result = await _produtoGateway.ObterProdutoAsync(produtoId, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(produtoId, result.Id);
            Assert.Equal("Produto Teste", result.Nome);
            Assert.Equal("Descrição Teste", result.Descricao);
            Assert.Equal(100.00m, result.Preco);
            Assert.Equal(Categoria.Lanche, result.Categoria);
            Assert.True(result.Ativo);
        }

        [Fact]
        public async Task ObterTodosProdutosAsync_DeveRetornarListaDeProdutos()
        {
            // Arrange
            var produtoDbList = new List<ProdutoDb>
                {
                    new() {
                        Id = Guid.NewGuid(),
                        Nome = "Produto Teste 1",
                        Descricao = "Descrição Teste 1",
                        Preco = 100.00m,
                        Categoria = Categoria.Lanche.ToString(),
                        Ativo = true
                    },
                    new() {
                        Id = Guid.NewGuid(),
                        Nome = "Produto Teste 2",
                        Descricao = "Descrição Teste 2",
                        Preco = 200.00m,
                        Categoria = Categoria.Lanche.ToString(),
                        Ativo = false
                    }
                };
            _produtoRepositoryMock.Setup(x => x.ObterTodosProdutosAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(produtoDbList);

            // Act
            var result = await _produtoGateway.ObterTodosProdutosAsync(CancellationToken.None);

            // Assert
            Assert.Equal(2, result.Count());
            Assert.All(result, p => Assert.NotNull(p));
        }

        [Fact]
        public async Task ObterProdutosCategoriaAsync_DeveRetornarListaDeProdutosPorCategoria()
        {
            // Arrange
            var categoria = Categoria.Lanche;
            var produtoDbList = new List<ProdutoDb>
                {
                    new() {
                        Id = Guid.NewGuid(),
                        Nome = "Produto Teste 1",
                        Descricao = "Descrição Teste 1",
                        Preco = 100.00m,
                        Categoria = Categoria.Lanche.ToString(),
                        Ativo = true
                    }
                };
            _produtoRepositoryMock.Setup(x => x.ObterProdutosCategoriaAsync(categoria.ToString(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(produtoDbList);

            // Act
            var result = await _produtoGateway.ObterProdutosCategoriaAsync(categoria, CancellationToken.None);

            // Assert
            Assert.Single(result);
            Assert.Equal(categoria, result.First().Categoria);
        }
    }
}