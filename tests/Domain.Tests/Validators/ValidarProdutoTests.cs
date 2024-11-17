using Domain.Entities;
using Domain.ValueObjects;
using FluentValidation.TestHelper;

namespace Domain.Tests.Validators
{
    public class ValidarProdutoTests
    {
        private readonly ValidarProduto _validarProduto;

        public ValidarProdutoTests() => _validarProduto = new ValidarProduto();

        [Fact]
        public void DeveSerValido_QuandoInformacoesCorretas()
        {
            // Arrange
            var produto = new Produto(Guid.NewGuid(), "Produto Exemplo", "Descrição do Produto", 100.00m, Categoria.Lanche, true);

            // Act
            var result = _validarProduto.TestValidate(produto);

            // Assert
            result.ShouldNotHaveValidationErrorFor(p => p.Nome);
            result.ShouldNotHaveValidationErrorFor(p => p.Descricao);
            result.ShouldNotHaveValidationErrorFor(p => p.Preco);
            result.ShouldNotHaveValidationErrorFor(p => p.Categoria);
            result.ShouldNotHaveValidationErrorFor(p => p.Ativo);
        }

        [Fact]
        public void DeveSerInvalido_QuandoNomeInvalido()
        {
            // Arrange
            var produto = new Produto(Guid.NewGuid(), "", "Descrição do Produto", 100.00m, Categoria.Lanche, true);

            // Act
            var result = _validarProduto.TestValidate(produto);

            // Assert
            result.ShouldHaveValidationErrorFor(p => p.Nome);
        }

        [Fact]
        public void DeveSerInvalido_QuandoPrecoInvalido()
        {
            // Arrange
            var produto = new Produto(Guid.NewGuid(), "Produto Exemplo", "Descrição do Produto", 0.00m, Categoria.Lanche, true);

            // Act
            var result = _validarProduto.TestValidate(produto);

            // Assert
            result.ShouldHaveValidationErrorFor(p => p.Preco);
        }
    }
}