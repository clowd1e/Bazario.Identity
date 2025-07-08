using Bazario.AspNetCore.Shared.Abstractions.Mappers;
using Bazario.Identity.Application.Features.Auth.DTO.RequestModels;
using Bazario.Identity.Application.Helpers;
using Bazario.Identity.Application.Identity;

namespace Bazario.Identity.Application.Features.Auth.Mappers
{
    internal sealed class UpdateApplicationUserRequestMapper
        : Mapper<UpdateApplicationUserRequestModel, ApplicationUser>
    {
        public override ApplicationUser Map(UpdateApplicationUserRequestModel source)
        {
            var applicationUser = source.ApplicationUser;

            applicationUser.Email = source.Email;
            applicationUser.NormalizedEmail = source.Email.ToUpperInvariant();
            applicationUser.PhoneNumber = source.PhoneNumber;

            var username = UsernameGenerator.Generate(applicationUser.Id);

            applicationUser.UserName = username;
            applicationUser.NormalizedUserName = username.ToUpperInvariant();

            return applicationUser;
        }
    }
}
