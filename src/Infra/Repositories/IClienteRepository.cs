using Core.Domain.Data;
using Infra.Dto;

namespace Infra.Repositories
{
    public interface IClienteRepository : IRepositoryGeneric<ClienteDb>
    {
        Task<IEnumerable<ClienteDb>> ObterTodosClientesAsync(CancellationToken cancellationToken);
    }
}
