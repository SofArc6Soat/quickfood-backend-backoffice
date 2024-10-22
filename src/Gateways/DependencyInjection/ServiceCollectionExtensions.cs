using Infra.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace Gateways.DependencyInjection
{
    [ExcludeFromCodeCoverage]
    public static class ServiceCollectionExtensions
    {
        public static void AddGatewayDependencyServices(this IServiceCollection services, string connectionString)
        {
            services.AddScoped<IClienteGateway, ClienteGateway>();
            services.AddScoped<IProdutoGateway, ProdutoGateway>();
            services.AddScoped<IFuncionarioGateway, FuncionarioGateway>();

            services.AddInfraDependencyServices(connectionString);
        }
    }
}