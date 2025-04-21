using Bazario.Identity.Infrastructure.Extensions.DI;
using Microsoft.Extensions.DependencyInjection;

namespace Bazario.Identity.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.ConfigureAppOptions();

            services.AddPersistence();

            return services;
        }
    }
}
