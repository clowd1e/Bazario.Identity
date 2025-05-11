using Bazario.AspNetCore.Shared.Domain.Common.Users;
using Bazario.Identity.Domain.ConfirmEmailTokens.ValueObjects;

namespace Bazario.Identity.Domain.ConfirmEmailTokens
{
    public interface IConfirmEmailTokenRepository
    {
        Task<ConfirmEmailToken?> GetByIdWithUserAsync(
            ConfirmEmailTokenId confirmEmailTokenId,
            CancellationToken cancellationToken = default);

        Task InsertAsync(
            ConfirmEmailToken confirmEmailToken,
            CancellationToken cancellationToken = default);

        Task DeleteAsync(
            ConfirmEmailToken confirmEmailToken);

        Task<int> CountActiveTokensAsync(
            UserId userId,
            CancellationToken cancellationToken);
    }
}
