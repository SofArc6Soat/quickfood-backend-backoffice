using Cora.Infra.Repository;
using Infra.Context;
using Infra.Dto;
using Microsoft.EntityFrameworkCore;

namespace Infra.Repositories
{
    public class ClienteRepository(ApplicationDbContext context) : RepositoryGeneric<ClienteDb>(context), IClienteRepository
    {
        private readonly DbSet<ClienteDb> _clientes = context.Set<ClienteDb>();

        public async Task<IEnumerable<ClienteDb>> ObterTodosClientesAsync(CancellationToken cancellationToken) =>
            await _clientes.AsNoTracking().Where(p => p.Ativo).ToListAsync(cancellationToken);
    }
}
