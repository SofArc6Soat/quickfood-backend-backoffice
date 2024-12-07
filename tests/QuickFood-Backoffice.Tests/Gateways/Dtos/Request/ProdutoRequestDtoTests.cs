using Gateways.Dtos.Request;
using System.ComponentModel.DataAnnotations;

namespace QuickFood_Backoffice.Tests.Gateways.Dtos.Request;

public class ProdutoRequestDtoTests
{
    private ProdutoRequestDto CriarProdutoValido()
    {
        return new ProdutoRequestDto
        {
            Id = Guid.NewGuid(),
            Nome = "Produto Exemplo",
            Descricao = "Descrição do Produto Exemplo",
            Preco = 100.00m,
            Ativo = true,
            Categoria = "Lanche"
        };
    }

    private List<ValidationResult> ValidarModelo(object modelo)
    {
        var contexto = new ValidationContext(modelo, null, null);
        var resultados = new List<ValidationResult>();
        Validator.TryValidateObject(modelo, contexto, resultados, true);
        return resultados;
    }

    [Fact]
    public void ProdutoRequestDto_Valido_DevePassarValidacao()
    {
        // Arrange
        var produto = CriarProdutoValido();

        // Act
        var resultados = ValidarModelo(produto);

        // Assert
        Assert.Empty(resultados);
    }

    [Fact]
    public void ProdutoRequestDto_IdInvalido_DeveFalharValidacao()
    {
        // Arrange
        var produto = CriarProdutoValido();
        produto.Id = Guid.Empty;

        // Act
        var resultados = ValidarModelo(produto);

        // Assert
        Assert.Contains(resultados, r => r.ErrorMessage.Contains("O campo Id é obrigatório."));
    }

    [Fact]
    public void ProdutoRequestDto_NomeInvalido_DeveFalharValidacao()
    {
        // Arrange
        var produto = CriarProdutoValido();
        produto.Nome = "A";

        // Act
        var resultados = ValidarModelo(produto);

        // Assert
        Assert.Contains(resultados, r => r.ErrorMessage.Contains("O campo Nome deve conter entre 2 e 40 caracteres."));
    }

    [Fact]
    public void ProdutoRequestDto_DescricaoInvalida_DeveFalharValidacao()
    {
        // Arrange
        var produto = CriarProdutoValido();
        produto.Descricao = "Desc";

        // Act
        var resultados = ValidarModelo(produto);

        // Assert
        Assert.Contains(resultados, r => r.ErrorMessage.Contains("O campo Descrição deve conter entre 5 e 200 caracteres."));
    }

    [Fact]
    public void ProdutoRequestDto_PrecoInvalido_DeveFalharValidacao()
    {
        // Arrange
        var produto = CriarProdutoValido();
        produto.Preco = 0;

        // Act
        var resultados = ValidarModelo(produto);

        // Assert
        Assert.Contains(resultados, r => r.ErrorMessage.Contains("O campo Preço deve ter o valor entre 1 e 9999."));
    }

    [Fact]
    public void ProdutoRequestDto_CategoriaInvalida_DeveFalharValidacao()
    {
        // Arrange
        var produto = CriarProdutoValido();
        produto.Categoria = "Invalida";

        // Act
        var resultados = ValidarModelo(produto);

        // Assert
        Assert.Contains(resultados, r => r.ErrorMessage.Contains("Categoria inválida."));
    }
}