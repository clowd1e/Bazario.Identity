using Bazario.AspNetCore.Shared.Application.Mappers.DependencyInjection;
using Bazario.AspNetCore.Shared.Results;
using Bazario.Identity.Application.Features.Auth.Commands.RegisterUser;
using Bazario.Identity.Application.Features.Auth.DTO.RequestModels;
using Bazario.Identity.Application.Features.Auth.Mappers;
using Bazario.Identity.Application.Identity;
using Bazario.Identity.Domain.ConfirmEmailTokens;
using Bazario.Identity.Domain.Users;
using Microsoft.Extensions.DependencyInjection;

namespace Bazario.Identity.Application.Extensions.DI
{
    public static class MapperExtensions
    {
        public static IServiceCollection AddMappers(this IServiceCollection services)
        {
            services.AddMapper<RegisterUserCommand, ApplicationUser, RegisterUserCommandMapper>();
            services.AddMapper<ApplicationUser, Result<User>, ApplicationUserToDomainUserMapper>();
            services.AddMapper<CreateConfirmEmailTokenRequestModel, Result<ConfirmEmailToken>, CreateConfirmEmailTokenRequestMapper>();

            return services;
        }
    }
}
