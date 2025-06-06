using Bazario.AspNetCore.Shared.Authentication.Options;
using Bazario.AspNetCore.Shared.Infrastructure.MessageBroker.Options;
using Bazario.AspNetCore.Shared.Infrastructure.Persistence.Options;
using Bazario.AspNetCore.Shared.Infrastructure.Persistence.Outbox.Options;
using Bazario.AspNetCore.Shared.Options.DependencyInjection;
using Bazario.Identity.Application.Identity.Options;
using Bazario.Identity.Application.Identity.Options.ConfirmEmailToken;
using Bazario.Identity.Application.Identity.Options.Login;
using Bazario.Identity.Application.Identity.Options.RefreshToken;
using Bazario.Identity.Infrastructure.BackgroundJobs.Options;
using Bazario.Identity.Infrastructure.Services.Emails.Options;
using Bazario.Identity.Infrastructure.Services.Security.Options;
using Microsoft.Extensions.DependencyInjection;

namespace Bazario.Identity.Infrastructure.Extensions.DI
{
    public static class AppOptionsExtensions
    {
        public static IServiceCollection ConfigureAppOptions(this IServiceCollection services)
        {
            services.ConfigureValidatableOptions<LoginSettings, LoginSettingsValidator>(
                LoginSettings.SectionName);
            services.ConfigureValidatableOptions<DbSettings, DbSettingsValidator>(
                DbSettings.SectionName);
            services.ConfigureValidatableOptions<JwtSettings, JwtSettingsValidator>(
                JwtSettings.SectionName);
            services.ConfigureValidatableOptions<RefreshTokenSettings, RefreshTokenSettingsValidator>(
                RefreshTokenSettings.SectionName);
            services.ConfigureValidatableOptions<HashSettings, HashSettingsValidator>(
                HashSettings.SectionName);
            services.ConfigureValidatableOptions<ConfirmEmailTokenSettings, ConfirmEmailTokenSettingsValidator>(
                ConfirmEmailTokenSettings.SectionName);
            services.ConfigureValidatableOptions<OutboxSettings, OutboxSettingsValidator>(
                OutboxSettings.SectionName);
            services.ConfigureValidatableOptions<MessageBrokerSettings, MessageBrokerSettingsValidator>(
                MessageBrokerSettings.SectionName);
            services.ConfigureValidatableOptions<EmailLinkGeneratorSettings, EmailLinkGeneratorSettingsValidator>(
                EmailLinkGeneratorSettings.SectionName);
            services.ConfigureValidatableOptions<ExpiredRefreshTokensRemovalSettings, ExpiredRefreshTokensRemovalSettingsValidator>(
                ExpiredRefreshTokensRemovalSettings.SectionName);

            return services;
        }
    }
}
