using Bazario.AspNetCore.Shared.Infrastructure.Services.DependencyInjection;
using Bazario.Identity.Application.Abstractions.Identity;
using Bazario.Identity.Application.Abstractions.Security;
using Bazario.Identity.Infrastructure.Services.Authentication;
using Bazario.Identity.Infrastructure.Services.Identity;
using Bazario.Identity.Infrastructure.Services.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Bazario.Identity.Infrastructure.Extensions.DI
{
    public static class InfrastructureServicesExtensions
    {
        public static IServiceCollection AddInfrastructureServices(
            this IServiceCollection services)
        {
            services.AddUserContextServiceWithHttpContextAccessor();
            services.AddScoped<ITokenService, JwtTokenService>();
            services.AddScoped<IIdentityService, IdentityService>();
            services.AddScoped<IRoleService<IdentityRole>, RoleService<IdentityRole>>();

            services.AddScoped<IHasher, HmacSha256Hasher>();

            return services;
        }
    }
}
