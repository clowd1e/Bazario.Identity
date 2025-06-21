using Bazario.AspNetCore.Shared.Domain.Common.Users.Roles;
using Bazario.AspNetCore.Shared.Results;
using Bazario.Identity.Application.Identity;

namespace Bazario.Identity.Application.Abstractions.Identity
{
    public interface IIdentityService
    {
        Task<Result> LoginAsync(ApplicationUser user, string password);

        Task<Role> GetUserRoleAsync(ApplicationUser user);

        Task CreateAsync(ApplicationUser user, string password);

        Task CreateWithoutPasswordAsync(ApplicationUser user);

        Task DeleteAsync(ApplicationUser user);

        Task AssignUserToRoleAsync(ApplicationUser user, string roleName);

        Task<ApplicationUser?> GetByIdAsync(string userId);

        Task<ApplicationUser?> GetByEmailAsync(string email);

        Task<bool> IsEmailConfirmed(ApplicationUser user);

        Result ConfirmEmail(ApplicationUser user);

        Task<Result> UpdateAsync(ApplicationUser user);
    }
}
