using Bazario.AspNetCore.Shared.Domain.Common.Users;
using Bazario.AspNetCore.Shared.Domain.Common.Users.Emails;
using Bazario.Identity.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bazario.Identity.Infrastructure.Persistence.Configurations
{
    internal sealed class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasIndex(user => user.Id)
                .IsUnique();

            builder.Property(user => user.Id)
                .HasConversion(
                    userId => userId.Value,
                    value => new UserId(value));

            builder.Property(user => user.Email)
                .HasConversion(
                    email => email.Value,
                    value => Email.Create(value).Value)
                .IsRequired();

            builder.HasMany(user => user.ConfirmEmailTokens)
                .WithOne(confirmEmailToken => confirmEmailToken.User);
        }
    }
}
