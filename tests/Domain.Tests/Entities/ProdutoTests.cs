using Domain.Entities;
using Domain.ValueObjects;

namespace Domain.Tests.Entities
{
    public class ProdutoTests
    {
        [Fact]
        public void CriarProduto_DeveCriarProdutoComSucesso()
        {
            // Arrange
            var id = Guid.NewGuid();
            var nome = "Produto Teste";
            var descricao = "Descrição Teste";
            var preco = 100.00m;
            var categoria = Categoria.Lanche;
            var ativo = true;

            // Act
            var produto = new Produto(id, nome, descricao, preco, categoria, ativo);

            // Assert
            Assert.Equal(id, produto.Id);
            Assert.Equal(nome, produto.Nome);
            Assert.Equal(descricao, produto.Descricao);
            Assert.Equal(preco, produto.Preco);
            Assert.Equal(categoria, produto.Categoria);
            Assert.Equal(ativo, produto.Ativo);
        }
    }
}