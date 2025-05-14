using Bazario.AspNetCore.Shared.Domain;
using Bazario.AspNetCore.Shared.Infrastructure.Persistence;
using Bazario.AspNetCore.Shared.Infrastructure.Persistence.Outbox;
using Bazario.Identity.Application.Identity;
using Bazario.Identity.Domain.ConfirmEmailTokens;
using Bazario.Identity.Domain.Users;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Bazario.Identity.Infrastructure.Persistence
{
    public sealed class AppDbContext : IdentityDbContext<ApplicationUser>, IHasOutboxMessages
    {
        public DbSet<OutboxMessage> OutboxMessages { get; init; }
        
        public new DbSet<User> Users { get; init; }

        public DbSet<ConfirmEmailToken> ConfirmEmailTokens { get; init; }

        public AppDbContext(
            DbContextOptions<AppDbContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Ignore<DomainEvent>();

            builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }
    }
}
