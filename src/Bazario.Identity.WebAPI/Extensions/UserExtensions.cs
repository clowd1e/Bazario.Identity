using Bazario.AspNetCore.Shared.Abstractions.Data;
using Bazario.AspNetCore.Shared.Application.Mappers;
using Bazario.AspNetCore.Shared.Domain.Common.Users.Roles;
using Bazario.AspNetCore.Shared.Options;
using Bazario.AspNetCore.Shared.Results;
using Bazario.Identity.Application.Abstractions.Identity;
using Bazario.Identity.Application.Identity;
using Bazario.Identity.Domain.Users;
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

            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

            var userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();

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

            var userMapper = scope.ServiceProvider.GetRequiredService<Mapper<ApplicationUser, Result<User>>>();

            var domainUserResult = userMapper.Map(owner);

            if (domainUserResult.IsFailure)
            {
                throw new InvalidOperationException($"Failed to map ApplicationUser to User: {domainUserResult.Error}");
            }

            var domainUser = domainUserResult.Value;

            await userRepository.InsertAsync(domainUser);

            await unitOfWork.SaveChangesAsync();

            await identityService.CreateAsync(
                owner,
                ownerSettings.Password);

            await identityService.AssignUserToRoleAsync(
                owner,
                Role.Owner.ToString());
        }
    }
}
