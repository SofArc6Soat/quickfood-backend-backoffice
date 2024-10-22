using Cora.Infra.Repository;
using Infra.Context;
using Infra.Dto;

namespace Infra.Repositories
{
    public class FuncionarioRepository(ApplicationDbContext context) : RepositoryGeneric<FuncionarioDb>(context), IFuncionarioRepository
    {
    }
}
