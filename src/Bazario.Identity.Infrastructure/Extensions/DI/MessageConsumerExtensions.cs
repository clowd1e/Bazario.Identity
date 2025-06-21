using Bazario.AspNetCore.Shared.Contracts.UserUpdated;
using Bazario.AspNetCore.Shared.Infrastructure.MessageBroker.DependencyInjection;
using Bazario.Identity.Infrastructure.Consumers;
using Microsoft.Extensions.DependencyInjection;

namespace Bazario.Identity.Infrastructure.Extensions.DI
{
    public static class MessageConsumerExtensions
    {
        public static IServiceCollection AddMessageConsumers(this IServiceCollection services)
        {
            services.AddMessageConsumer<UserUpdatedForIdentityServiceEvent, UserUpdatedForIdentityServiceEventConsumer>();

            return services;
        }
    }
}
