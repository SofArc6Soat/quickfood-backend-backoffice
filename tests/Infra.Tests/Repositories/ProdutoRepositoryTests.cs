using Domain.Entities;
using Domain.ValueObjects;
using FluentAssertions;
using Gateways;
using Infra.Dto;
using Infra.Repositories;
using Moq;

namespace Infra.Tests.Repositories
{
    public class ProdutoRepositoryTests
    {
        [Fact]
        public async Task DeveCadastrarProdutoComSucesso()
        {
            // Arrange
            var mockRepository = new Mock<IProdutoRepository>();
            var produto = new Produto(Guid.NewGuid(), "Produto Exemplo", "Descrição do Produto", 100.00m, Categoria.Lanche, true);

            mockRepository.Setup(repo => repo.InsertAsync(It.IsAny<ProdutoDb>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            mockRepository.Setup(repo => repo.UnitOfWork.CommitAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(true));

            var produtoGateway = new ProdutoGateway(mockRepository.Object);

            // Act
            var resultado = await produtoGateway.CadastrarProdutoAsync(produto, CancellationToken.None);

            // Assert
            resultado.Should().BeTrue();
        }
    }
}