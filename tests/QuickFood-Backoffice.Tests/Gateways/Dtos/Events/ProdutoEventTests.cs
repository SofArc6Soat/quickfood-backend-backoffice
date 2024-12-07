using Gateways.Dtos.Events;

namespace QuickFood_Backoffice.Tests.Gateways.Dtos.Events;

public class ProdutoEventTests
{
    [Fact]
    public void ProdutoCriadoEvent_DeveSerInicializadoCorretamente()
    {
        // Arrange
        var id = Guid.NewGuid();
        var nome = "Produto 1";
        var descricao = "Descrição do Produto 1";
        var preco = 10.0m;
        var categoria = "Categoria 1";
        var ativo = true;

        // Act
        var produtoCriadoEvent = new ProdutoCriadoEvent
        {
            Id = id,
            Nome = nome,
            Descricao = descricao,
            Preco = preco,
            Categoria = categoria,
            Ativo = ativo
        };

        // Assert
        Assert.Equal(id, produtoCriadoEvent.Id);
        Assert.Equal(nome, produtoCriadoEvent.Nome);
        Assert.Equal(descricao, produtoCriadoEvent.Descricao);
        Assert.Equal(preco, produtoCriadoEvent.Preco);
        Assert.Equal(categoria, produtoCriadoEvent.Categoria);
        Assert.True(produtoCriadoEvent.Ativo);
    }

    [Fact]
    public void ProdutoAtualizadoEvent_DeveSerInicializadoCorretamente()
    {
        // Arrange
        var id = Guid.NewGuid();
        var nome = "Produto 1";
        var descricao = "Descrição do Produto 1";
        var preco = 10.0m;
        var categoria = "Categoria 1";
        var ativo = true;

        // Act
        var produtoAtualizadoEvent = new ProdutoAtualizadoEvent
        {
            Id = id,
            Nome = nome,
            Descricao = descricao,
            Preco = preco,
            Categoria = categoria,
            Ativo = ativo
        };

        // Assert
        Assert.Equal(id, produtoAtualizadoEvent.Id);
        Assert.Equal(nome, produtoAtualizadoEvent.Nome);
        Assert.Equal(descricao, produtoAtualizadoEvent.Descricao);
        Assert.Equal(preco, produtoAtualizadoEvent.Preco);
        Assert.Equal(categoria, produtoAtualizadoEvent.Categoria);
        Assert.True(produtoAtualizadoEvent.Ativo);
    }

    [Fact]
    public void ProdutoExcluidoEvent_DeveSerInicializadoCorretamente()
    {
        // Arrange
        var id = Guid.NewGuid();

        // Act
        var produtoExcluidoEvent = new ProdutoExcluidoEvent
        {
            Id = id
        };

        // Assert
        Assert.Equal(id, produtoExcluidoEvent.Id);
    }

    [Fact]
    public void ProdutoCriadoEvent_DeveTerValoresPadrao()
    {
        // Act
        var produtoCriadoEvent = new ProdutoCriadoEvent();

        // Assert
        Assert.Equal(Guid.Empty, produtoCriadoEvent.Id);
        Assert.Equal(string.Empty, produtoCriadoEvent.Nome);
        Assert.Equal(string.Empty, produtoCriadoEvent.Descricao);
        Assert.Equal(0m, produtoCriadoEvent.Preco);
        Assert.Equal(string.Empty, produtoCriadoEvent.Categoria);
        Assert.False(produtoCriadoEvent.Ativo);
    }

    [Fact]
    public void ProdutoAtualizadoEvent_DeveTerValoresPadrao()
    {
        // Act
        var produtoAtualizadoEvent = new ProdutoAtualizadoEvent();

        // Assert
        Assert.Equal(Guid.Empty, produtoAtualizadoEvent.Id);
        Assert.Equal(string.Empty, produtoAtualizadoEvent.Nome);
        Assert.Equal(string.Empty, produtoAtualizadoEvent.Descricao);
        Assert.Equal(0m, produtoAtualizadoEvent.Preco);
        Assert.Equal(string.Empty, produtoAtualizadoEvent.Categoria);
        Assert.False(produtoAtualizadoEvent.Ativo);
    }

    [Fact]
    public void ProdutoExcluidoEvent_DeveTerValoresPadrao()
    {
        // Act
        var produtoExcluidoEvent = new ProdutoExcluidoEvent();

        // Assert
        Assert.Equal(Guid.Empty, produtoExcluidoEvent.Id);
    }

    [Fact]
    public void ProdutoAtualizadoEvent_DeveHerdarDeProdutoCriadoEvent()
    {
        // Arrange
        var id = Guid.NewGuid();
        var nome = "Produto 1";
        var descricao = "Descrição do Produto 1";
        var preco = 10.0m;
        var categoria = "Categoria 1";
        var ativo = true;

        // Act
        var produtoAtualizadoEvent = new ProdutoAtualizadoEvent
        {
            Id = id,
            Nome = nome,
            Descricao = descricao,
            Preco = preco,
            Categoria = categoria,
            Ativo = ativo
        };

        // Assert
        Assert.IsAssignableFrom<ProdutoCriadoEvent>(produtoAtualizadoEvent);
        Assert.Equal(id, produtoAtualizadoEvent.Id);
        Assert.Equal(nome, produtoAtualizadoEvent.Nome);
        Assert.Equal(descricao, produtoAtualizadoEvent.Descricao);
        Assert.Equal(preco, produtoAtualizadoEvent.Preco);
        Assert.Equal(categoria, produtoAtualizadoEvent.Categoria);
        Assert.True(produtoAtualizadoEvent.Ativo);
    }
}