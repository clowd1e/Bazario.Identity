using Bazario.AspNetCore.Shared.Infrastructure.MessageBroker.DependencyInjection;
using Bazario.Identity.Infrastructure.Extensions.DI;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Bazario.Identity.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            var assembly = Assembly.GetExecutingAssembly();

            services.ConfigureAppOptions();

            services.AddMessageBroker(assembly);

            services.AddInfrastructureServices();

            services.AddPersistence();

            services.AddRepositories();

            services.ConfigureAppBackgroundJobs();

            return services;
        }
    }
}
