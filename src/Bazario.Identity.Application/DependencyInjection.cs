using Bazario.AspNetCore.Shared.Application.Behaviors.Validation.DependencyInjection;
using Bazario.AspNetCore.Shared.Application.MediatR.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Bazario.Identity.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            var assembly = Assembly.GetExecutingAssembly();

            services.AddMediatRServices(assembly);

            services.AddValidationPipelineBehavior();

            services.AddValidators(assembly);

            return services;
        }
    }
}
