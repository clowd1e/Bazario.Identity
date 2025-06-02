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

        public async Task<User?> GetByIdAsync(
            UserId userId,
            CancellationToken cancellationToken = default)
        {
            return await _context.Users
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
