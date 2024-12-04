using Infra.Context;
using Infra.Dto;
using Infra.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infra.Tests.Repositories;

public class ClienteRepositoryTests : IDisposable
{
    private ClienteRepository _clienteRepository;
    private ApplicationDbContext _context;

    public ClienteRepositoryTests()
    {
        RecreateContext();
    }

    private void RecreateContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Garante um banco de dados único para cada teste
            .Options;

        _context = new ApplicationDbContext(options);
        _clienteRepository = new ClienteRepository(_context);
    }

    private void SeedDatabase()
    {
        var clientes = new List<ClienteDb>
        {
            new ClienteDb
            {
                Id = Guid.NewGuid(),
                Nome = "Cliente 1",
                Email = "cliente1@example.com",
                Cpf = "12345678901",
                Ativo = true
            },
            new ClienteDb
            {
                Id = Guid.NewGuid(),
                Nome = "Cliente 2",
                Email = "cliente2@example.com",
                Cpf = "23456789012",
                Ativo = true
            },
            new ClienteDb
            {
                Id = Guid.NewGuid(),
                Nome = "Cliente Inativo",
                Email = "clienteinativo@example.com",
                Cpf = "34567890123",
                Ativo = false
            }
        };

        _context.Set<ClienteDb>().AddRange(clientes);
        _context.SaveChanges();
    }

    [Fact]
    public async Task ObterTodosClientesAsync_DeveRetornarApenasClientesAtivos()
    {
        // Arrange
        RecreateContext();
        SeedDatabase();

        // Act
        var result = await _clienteRepository.ObterTodosClientesAsync(CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        Assert.All(result, p => Assert.True(p.Ativo));
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
