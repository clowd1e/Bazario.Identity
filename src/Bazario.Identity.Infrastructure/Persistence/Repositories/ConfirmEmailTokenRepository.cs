using Bazario.AspNetCore.Shared.Domain.Common.Users;
using Bazario.Identity.Domain.Common.Timestamps;
using Bazario.Identity.Domain.ConfirmEmailTokens;
using Bazario.Identity.Domain.ConfirmEmailTokens.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Bazario.Identity.Infrastructure.Persistence.Repositories
{
    public sealed class ConfirmEmailTokenRepository
        : IConfirmEmailTokenRepository
    {
        private readonly AppDbContext _context;

        public ConfirmEmailTokenRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<int> CountActiveTokensAsync(
            UserId userId,
            CancellationToken cancellationToken)
        {
            var activeTokens = await _context.ConfirmEmailTokens
                .Include(token => token.User)
                .AsNoTracking()
                .Where(token => token.User.Id == userId)
                .ToListAsync(cancellationToken);

            var activeTokensCount = activeTokens
                .Count(token => token.IsActive);

            return activeTokensCount;
        }

        public Task DeleteAsync(
            ConfirmEmailToken confirmEmailToken)
        {
            _context.ConfirmEmailTokens.Remove(confirmEmailToken);

            return Task.CompletedTask;
        }

        public async Task<ConfirmEmailToken?> GetByIdWithUserAsync(
            ConfirmEmailTokenId confirmEmailTokenId,
            CancellationToken cancellationToken = default)
        {
            return await _context.ConfirmEmailTokens
                .Include(token => token.User)
                .FirstOrDefaultAsync(
                    token => token.Id == confirmEmailTokenId,
                    cancellationToken);
        }

        public async Task<IEnumerable<ConfirmEmailToken>> GetExpiredConfirmEmailTokensAsync(
            CancellationToken cancellationToken = default)
        {
            return await _context.ConfirmEmailTokens
                .Where(token => token.ExpiresAt <= Timestamp.UtcNow())
                .ToListAsync(cancellationToken);
        }

        public async Task InsertAsync(
            ConfirmEmailToken confirmEmailToken,
            CancellationToken cancellationToken = default)
        {
            await _context.ConfirmEmailTokens
                .AddAsync(confirmEmailToken, cancellationToken);
        }
    }
}
