using Bazario.AspNetCore.Shared.Infrastructure.Options.DependencyInjection;
using Bazario.Identity.WebAPI.Options;

namespace Bazario.Identity.WebAPI.Extensions.DI
{
    public static class OptionsExtensions
    {
        public static IServiceCollection AddPresentationOptions(this IServiceCollection services)
        {
            services.ConfigureValidatableOptions<OwnerSettings, OwnerSettingsValidator>(
                OwnerSettings.SectionName);

            return services;
        }
    }
}
