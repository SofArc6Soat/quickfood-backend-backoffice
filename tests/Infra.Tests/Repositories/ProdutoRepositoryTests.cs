using Domain.Tests.TestHelpers;
using Domain.ValueObjects;
using Infra.Context;
using Infra.Dto;
using Infra.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infra.Tests.Repositories;

public class ProdutoRepositoryTests
{
    private readonly ProdutoRepository _produtoRepository;
    private readonly ApplicationDbContext _context;

    public ProdutoRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "ProdutoDatabase")
            .Options;

        _context = new ApplicationDbContext(options);
        _produtoRepository = new ProdutoRepository(_context);

        SeedDatabase();
    }

    private void SeedDatabase()
    {
        var produtos = new List<ProdutoDb>
        {
            new ProdutoDb
            {
                Id = Guid.NewGuid(),
                Nome = "Produto 1",
                Descricao = "Descrição 1",
                Preco = 100.00m,
                Categoria = Categoria.Lanche.ToString(),
                Ativo = true
            },
            new ProdutoDb
            {
                Id = Guid.NewGuid(),
                Nome = "Produto 2",
                Descricao = "Descrição 2",
                Preco = 150.00m,
                Categoria = Categoria.Bebida.ToString(),
                Ativo = true
            },
            new ProdutoDb
            {
                Id = Guid.NewGuid(),
                Nome = "Produto 3",
                Descricao = "Descrição 3",
                Preco = 200.00m,
                Categoria = Categoria.Sobremesa.ToString(),
                Ativo = true
            },
            new ProdutoDb
            {
                Id = Guid.NewGuid(),
                Nome = "Produto Inativo",
                Descricao = "Descrição do Produto Inativo",
                Preco = 250.00m,
                Categoria = Categoria.Acompanhamento.ToString(),
                Ativo = false
            }
        };

        _context.Set<ProdutoDb>().AddRange(produtos);
        _context.SaveChanges();
    }

    [Fact]
    public async Task ObterTodosProdutosAsync_DeveRetornarApenasProdutosAtivos()
    {
        // Act
        var result = await _produtoRepository.ObterTodosProdutosAsync(CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(3, result.Count());
        Assert.All(result, p => Assert.True(p.Ativo));
    }


    [Fact]
    public async Task ObterProdutosCategoriaAsync_DeveRetornarVazio_QuandoCategoriaNaoExistir()
    {
        // Act
        var result = await _produtoRepository.ObterProdutosCategoriaAsync("CategoriaInexistente", CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }
}
