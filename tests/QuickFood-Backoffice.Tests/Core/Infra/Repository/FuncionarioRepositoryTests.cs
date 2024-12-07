using Infra.Context;
using Infra.Dto;
using Infra.Repositories;
using Microsoft.EntityFrameworkCore;
using QuickFood_Backoffice.Tests.TestHelpers;

namespace QuickFood_Backoffice.Tests.Core.Infra.Repository;

public class FuncionarioRepositoryTests : IDisposable
{
    private FuncionarioRepository _funcionarioRepository;
    private ApplicationDbContext _context;

    public FuncionarioRepositoryTests()
    {
        RecreateContext();
    }

    private void RecreateContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Garante um banco de dados único para cada teste
            .Options;

        _context = new ApplicationDbContext(options);
        _funcionarioRepository = new FuncionarioRepository(_context);
    }

    private void SeedDatabase()
    {
        _context.Database.EnsureDeleted(); // Garantir que o banco de dados seja limpo antes de cada teste
        _context.Database.EnsureCreated();

        var funcionarios = new List<FuncionarioDb>
        {
            FuncionarioFakeDataFactory.CriarFuncionarioDbValido(),
            FuncionarioFakeDataFactory.CriarOutroFuncionarioDbValido(),
            FuncionarioFakeDataFactory.CriarFuncionarioDbInvalido()
        };

        _context.Set<FuncionarioDb>().AddRange(funcionarios);
        _context.SaveChanges();
    }

    [Fact]
    public async Task FindByIdAsync_DeveRetornarFuncionario_QuandoIdExistir()
    {
        // Arrange
        RecreateContext();
        SeedDatabase();
        var funcionarioId = _context.Set<FuncionarioDb>().First().Id;

        // Act
        var result = await _funcionarioRepository.FindByIdAsync(funcionarioId, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(funcionarioId, result?.Id);
    }

    [Fact]
    public async Task FindByIdAsync_DeveRetornarNull_QuandoIdNaoExistir()
    {
        // Arrange
        RecreateContext();
        SeedDatabase();
        var funcionarioId = Guid.NewGuid();

        // Act
        var result = await _funcionarioRepository.FindByIdAsync(funcionarioId, CancellationToken.None);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task InsertAsync_DeveAdicionarNovoFuncionario()
    {
        // Arrange
        RecreateContext();
        SeedDatabase();
        var novoFuncionario = FuncionarioFakeDataFactory.CriarFuncionarioDbValido();
        novoFuncionario.Id = Guid.NewGuid(); // Garantir que o ID seja único

        // Act
        await _funcionarioRepository.InsertAsync(novoFuncionario, CancellationToken.None);
        await _context.SaveChangesAsync(); // Garantir que a inserção seja persistida
        var result = await _funcionarioRepository.FindByIdAsync(novoFuncionario.Id, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(novoFuncionario.Id, result?.Id);
    }

    [Fact]
    public async Task UpdateAsync_DeveAtualizarFuncionarioExistente()
    {
        // Arrange
        RecreateContext();
        SeedDatabase();
        var funcionarioId = _context.Set<FuncionarioDb>().AsNoTracking().First().Id;
        var funcionario = await _funcionarioRepository.FindByIdAsync(funcionarioId, CancellationToken.None);
        funcionario.Nome = "Funcionario Atualizado";

        // Act
        _context.Entry(funcionario).State = EntityState.Detached; // Desanexar a entidade do contexto
        _context.Set<FuncionarioDb>().Update(funcionario); // Atualizar a entidade no contexto
        await _context.SaveChangesAsync(); // Garantir que a atualização seja persistida
        var result = await _funcionarioRepository.FindByIdAsync(funcionario.Id, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Funcionario Atualizado", result?.Nome);
    }

    [Fact]
    public async Task DeleteAsync_DeveRemoverFuncionarioExistente()
    {
        // Arrange
        RecreateContext();
        SeedDatabase();
        var funcionario = _context.Set<FuncionarioDb>().First();

        // Act
        await _funcionarioRepository.DeleteAsync(funcionario.Id, CancellationToken.None);
        await _context.SaveChangesAsync(); // Garantir que a exclusão seja persistida
        var result = await _funcionarioRepository.FindByIdAsync(funcionario.Id, CancellationToken.None);

        // Assert
        Assert.Null(result);
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}