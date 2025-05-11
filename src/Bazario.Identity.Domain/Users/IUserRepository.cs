using Bazario.AspNetCore.Shared.Domain.Common.Users;

namespace Bazario.Identity.Domain.Users
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(
            UserId userId,
            CancellationToken cancellationToken = default);

        Task InsertAsync(
            User user,
            CancellationToken cancellationToken = default);
    }
}
