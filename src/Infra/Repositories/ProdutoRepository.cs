using Cora.Infra.Repository;
using Infra.Context;
using Infra.Dto;
using Microsoft.EntityFrameworkCore;

namespace Infra.Repositories
{
    public class ProdutoRepository(ApplicationDbContext context) : RepositoryGeneric<ProdutoDb>(context), IProdutoRepository
    {
        private readonly DbSet<ProdutoDb> _produtos = context.Set<ProdutoDb>();

        public async Task<IEnumerable<ProdutoDb>> ObterTodosProdutosAsync(CancellationToken cancellationToken) =>
            await _produtos.AsNoTracking().Where(p => p.Ativo).ToListAsync(cancellationToken);

        public async Task<IEnumerable<ProdutoDb>> ObterProdutosCategoriaAsync(string categoria, CancellationToken cancellationToken) =>
            await _produtos.AsNoTracking().Where(p => p.Ativo && p.Categoria == categoria).ToListAsync(cancellationToken);
    }
}
