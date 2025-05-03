using Bazario.AspNetCore.Shared.Authorization.Roles;
using Bazario.AspNetCore.Shared.Results;
using Bazario.Identity.Application.Identity;

namespace Bazario.Identity.Application.Abstractions.Identity
{
    public interface IIdentityService
    {
        Task<Result> LoginAsync(User user, string password);

        Task PopulateRefreshTokenAsync(User user, string refreshToken);

        Task<Role> GetUserRoleAsync(User user);

        Result ValidateRefreshToken(User user);

        Task<Result<User>> GetByRefreshTokenAsync(string refreshToken);

        Task CreateAsync(User user, string password);

        Task AssignUserToRoleAsync(User user, string roleName);

        Task<User?> GetByEmailAsync(string email);

        Task<bool> IsEmailConfirmed(User user);
    }
}
