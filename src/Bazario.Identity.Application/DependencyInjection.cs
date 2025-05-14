using Bazario.AspNetCore.Shared.Application.Behaviors.Validation.DependencyInjection;
using Bazario.AspNetCore.Shared.Application.MediatR.DependencyInjection;
using Bazario.Identity.Application.Extensions.DI;
using Microsoft.Extensions.DependencyInjection;

namespace Bazario.Identity.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            var assembly = typeof(DependencyInjection).Assembly;

            services.AddMediatRServices(assembly);

            services.AddValidationPipelineBehavior();

            services.AddValidators(assembly);

            services.AddMappers();

            return services;
        }
    }
}
