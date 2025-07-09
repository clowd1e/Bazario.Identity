using Bazario.AspNetCore.Shared.Domain.Common.Users;
using Bazario.Identity.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace Bazario.Identity.Infrastructure.Persistence.Repositories
{
    public sealed class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public Task Delete(User user)
        {
            _context.Users.Remove(user);

            return Task.CompletedTask;
        }

        public async Task<User?> GetByIdAsync(
            UserId userId,
            CancellationToken cancellationToken = default)
        {
            return await _context.Users
                .FirstOrDefaultAsync(
                    user => user.Id == userId,
                    cancellationToken);
        }

        public async Task<User?> GetByIdWithConfirmEmailTokensAsync(
            UserId userId,
            CancellationToken cancellationToken = default)
        {
            return await _context.Users
                .Include(user => user.ConfirmEmailTokens)
                .FirstOrDefaultAsync(
                    user => user.Id == userId,
                    cancellationToken);
        }

        public async Task InsertAsync(
            User user,
            CancellationToken cancellationToken = default)
        {
            await _context.Users.AddAsync(user, cancellationToken);
        }
    }
}
