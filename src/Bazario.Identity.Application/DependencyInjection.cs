using Bazario.AspNetCore.Shared.Application.Behaviors.Validation.DependencyInjection;
using Bazario.AspNetCore.Shared.Application.DomainEvents.DependencyInjection;
using Bazario.AspNetCore.Shared.Application.Messaging.DependencyInjection;
using Bazario.Identity.Application.Extensions.DI;
using Microsoft.Extensions.DependencyInjection;

namespace Bazario.Identity.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            var assembly = typeof(DependencyInjection).Assembly;

            services.AddMessaging(assembly);

            services.AddDomainEventHandlers(assembly);

            services.AddValidationPipelineBehavior();

            services.AddValidators(assembly);

            services.AddMappers();

            return services;
        }
    }
}
