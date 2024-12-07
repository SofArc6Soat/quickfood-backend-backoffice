using Domain.Entities;
using Domain.ValueObjects;
using Infra.Dto;

namespace QuickFood_Backoffice.Tests.TestHelpers;

public static class ProdutoFakeDataFactory
{
    public static Produto CriarProdutoValido() => new(ObterGuid(), "Produto Exemplo", "Descrição do Produto", 100.00m, Categoria.Lanche, true);

    public static Produto AlterarProdutoValido() => new(ObterGuid(), "Produto Exemplo 2", "Descrição do Produto 3", 120.00m, Categoria.Lanche, true);

    public static Produto CriarProdutoInvalido() => new(ObterGuidInvalido(), "A", "A", 00.00m, Categoria.Lanche, true);

    public static ProdutoDb CriarProdutoDbValido() => new()
    {
        Id = ObterGuid(),
        Nome = "Produto Exemplo",
        Descricao = "Descrição do Produto",
        Preco = 100.00m,
        Categoria = Categoria.Lanche.ToString(),
        Ativo = true
    };

    public static ProdutoDb CriarOutroProdutoDbValido() => new()
    {
        Id = ObterOutroGuid(),
        Nome = "Produto Exemplo 2",
        Descricao = "Descrição do Produto 2",
        Preco = 150.00m,
        Categoria = Categoria.Bebida.ToString(),
        Ativo = true
    };

    public static List<ProdutoDb> CriarListaProdutosDbValidos() => new()
    {
        CriarProdutoDbValido(),
        CriarOutroProdutoDbValido()
    };

    public static Guid ObterGuid() => Guid.Parse("d290f1ee-6c54-4b01-90e6-d701748f0851");

    public static Guid ObterOutroGuid() => Guid.Parse("a290f1ee-6c54-4b01-90e6-d701748f0853");

    public static Guid ObterGuidInvalido() => Guid.Parse("e290f1ee-6c54-4b01-90e6-d701748f0852");
}
