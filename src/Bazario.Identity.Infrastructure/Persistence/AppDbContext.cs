using Bazario.Identity.Application.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Bazario.Identity.Infrastructure.Persistence
{
    public sealed class AppDbContext : IdentityDbContext<User>
    {
        public AppDbContext(
            DbContextOptions<AppDbContext> options)
            : base(options) { }
    }
}
