using Bazario.AspNetCore.Shared.Authentication.Options;
using Bazario.AspNetCore.Shared.Infrastructure.Options.DependencyInjection;
using Bazario.Identity.Infrastructure.Authentication.Options;
using Bazario.Identity.Infrastructure.Persistence.Options;
using Bazario.Identity.Infrastructure.Services.Security.Options;
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
            services.ConfigureValidatableOptions<RefreshTokenSettings, RefreshTokenSettingsValidator>(
                RefreshTokenSettings.SectionName);
            services.ConfigureValidatableOptions<HashSettings, HashSettingsValidator>(
                HashSettings.SectionName);

            return services;
        }
    }
}
