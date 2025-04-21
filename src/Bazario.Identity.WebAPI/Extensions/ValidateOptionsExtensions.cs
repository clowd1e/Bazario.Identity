using Bazario.AspNetCore.Shared.Infrastructure.Options.DependencyInjection;
using Bazario.Identity.Infrastructure.Persistence.Options;

namespace Bazario.Identity.WebAPI.Extensions
{
    public static class ValidateOptionsExtensions
    {
        public static IServiceProvider ValidateAppOptions(this IServiceProvider serviceProvider)
        {
            serviceProvider.ValidateOptionsOnStart<DbSettings>();

            return serviceProvider;
        }
    }
}
