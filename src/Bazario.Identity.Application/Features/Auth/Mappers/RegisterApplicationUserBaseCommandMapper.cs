using Bazario.AspNetCore.Shared.Abstractions.Mappers;
using Bazario.Identity.Application.Features.Auth.DTO;
using Bazario.Identity.Application.Helpers;
using Bazario.Identity.Application.Identity;

namespace Bazario.Identity.Application.Features.Auth.Mappers
{
    internal sealed class RegisterApplicationUserBaseCommandMapper
        : Mapper<IRegisterApplicationUserBaseCommand, ApplicationUser>
    {
        public override ApplicationUser Map(IRegisterApplicationUserBaseCommand source)
        {
            var userId = Guid.NewGuid().ToString();

            var username = UsernameGenerator.Generate(userId);

            return new ApplicationUser
            {
                Id = userId,
                UserName = username,
                Email = source.Email,
                NormalizedEmail = source.Email.ToUpperInvariant(),
                NormalizedUserName = username.ToUpperInvariant(),
                PhoneNumber = source.PhoneNumber
            };
        }
    }
}
