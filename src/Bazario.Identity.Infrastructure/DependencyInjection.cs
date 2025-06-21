using Bazario.AspNetCore.Shared.Infrastructure.MessageBroker.DependencyInjection;
using Bazario.Identity.Infrastructure.Extensions.DI;
using Microsoft.Extensions.DependencyInjection;

namespace Bazario.Identity.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.ConfigureAppOptions();

            services.AddMessageBroker();

            services.AddInfrastructureServices();

            services.AddPersistence();

            services.AddRepositories();

            services.ConfigureAppBackgroundJobs();

            services.AddMessageConsumers();

            return services;
        }
    }
}
