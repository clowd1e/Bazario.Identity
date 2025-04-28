using Bazario.AspNetCore.Shared.Auth.Options;
using Bazario.AspNetCore.Shared.Infrastructure.Options.DependencyInjection;
using Bazario.Identity.Infrastructure.Persistence.Options;
using Microsoft.Extensions.DependencyInjection;

namespace Bazario.Identity.Infrastructure.Extensions.DI
{
    public static class AppOptionsExtensions
    {
        public static IServiceCollection ConfigureAppOptions(this IServiceCollection services)
        {
            services.ConfigureValidatableOptions<DbSettings, DbSettingsValidator>(
                DbSettings.SectionName);
            services.ConfigureValidatableOptions<JwtSettings, JwtSettingsValidator>(
                JwtSettings.SectionName);

            return services;
        }
    }
}
