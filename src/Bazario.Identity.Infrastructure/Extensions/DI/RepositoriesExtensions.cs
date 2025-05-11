using Bazario.Identity.Domain.ConfirmEmailTokens;
using Bazario.Identity.Domain.Users;
using Bazario.Identity.Infrastructure.Persistence.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Bazario.Identity.Infrastructure.Extensions.DI
{
    public static class RepositoriesExtensions
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IConfirmEmailTokenRepository, ConfirmEmailTokenRepository>();

            return services;
        }
    }
}
