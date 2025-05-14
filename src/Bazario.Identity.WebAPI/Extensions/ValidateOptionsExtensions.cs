using Bazario.AspNetCore.Shared.Authentication.Options;
using Bazario.AspNetCore.Shared.Infrastructure.MessageBroker.Options;
using Bazario.AspNetCore.Shared.Infrastructure.Persistence.Options;
using Bazario.AspNetCore.Shared.Infrastructure.Persistence.Outbox.Options;
using Bazario.AspNetCore.Shared.Options.DependencyInjection;
using Bazario.Identity.Application.Identity.Options;
using Bazario.Identity.Infrastructure.Authentication.Options;
using Bazario.Identity.Infrastructure.Services.Security.Options;
using Bazario.Identity.WebAPI.Options;

namespace Bazario.Identity.WebAPI.Extensions
{
    public static class ValidateOptionsExtensions
    {
        public static IServiceProvider ValidateAppOptions(this IServiceProvider serviceProvider)
        {
            serviceProvider.ValidateOptionsOnStart<DbSettings>();
            serviceProvider.ValidateOptionsOnStart<JwtSettings>();
            serviceProvider.ValidateOptionsOnStart<RefreshTokenSettings>();
            serviceProvider.ValidateOptionsOnStart<HashSettings>();
            serviceProvider.ValidateOptionsOnStart<OwnerSettings>();
            serviceProvider.ValidateOptionsOnStart<ConfirmEmailTokenSettings>();
            serviceProvider.ValidateOptionsOnStart<OutboxSettings>();
            serviceProvider.ValidateOptionsOnStart<MessageBrokerSettings>();

            return serviceProvider;
        }
    }
}
