using Bazario.AspNetCore.Shared.Domain.Common.Users.Roles;
using Bazario.AspNetCore.Shared.Options;
using Bazario.Identity.Application.Abstractions.Identity;
using Bazario.Identity.Application.Identity;
using Bazario.Identity.WebAPI.Options;

namespace Bazario.Identity.WebAPI.Extensions
{
    public static class UserExtensions
    {
        public async static Task SeedOwnerIfNotSeeded(this IApplicationBuilder app)
        {
            using IServiceScope scope = app.ApplicationServices.CreateScope();

            var ownerSettings = scope.ServiceProvider.GetOptions<OwnerSettings>();

            var identityService = scope.ServiceProvider.GetRequiredService<IIdentityService>();

            var owner = new ApplicationUser()
            {
                Id = Guid.NewGuid().ToString(),
                Email = ownerSettings.Email,
                UserName = ownerSettings.Username,
                EmailConfirmed = true
            };

            var ownerExists = await identityService.GetByEmailAsync(owner.Email);

            if (ownerExists is not null)
            {
                return;
            }

            await identityService.CreateAsync(
                owner,
                ownerSettings.Password);

            await identityService.AssignUserToRoleAsync(
                owner,
                Role.Owner.ToString());
        }
    }
}
