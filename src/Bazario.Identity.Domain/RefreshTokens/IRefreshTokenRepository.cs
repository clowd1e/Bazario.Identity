using Bazario.AspNetCore.Shared.Domain.Common.Users;
using Bazario.Identity.Domain.RefreshTokens.ValueObjects;

namespace Bazario.Identity.Domain.RefreshTokens
{
    public interface IRefreshTokenRepository
    {
        Task<RefreshToken?> GetBySessionIdWithUserAsync(
            SessionId sessionId,
            UserId userId,
            CancellationToken cancellationToken = default);

        Task InsertAsync(
            RefreshToken refreshToken,
            CancellationToken cancellationToken = default);

        Task<int> GetUserSessionsCountAsync(
            UserId userId,
            CancellationToken cancellationToken = default);

        Task<IEnumerable<RefreshToken>> GetExpiredRefreshTokensAsync(
            CancellationToken cancellationToken = default);

        Task DeleteAsync(RefreshToken refreshToken);
    }
}
