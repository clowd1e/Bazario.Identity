using Bazario.Identity.Domain.Common.Timestamps;
using Bazario.Identity.Domain.Common.TokenHashes;
using Bazario.Identity.Domain.ConfirmEmailTokens;
using Bazario.Identity.Domain.ConfirmEmailTokens.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bazario.Identity.Infrastructure.Persistence.Configurations
{
    internal sealed class ConfirmEmailTokenConfiguration
        : IEntityTypeConfiguration<ConfirmEmailToken>
    {
        public void Configure(EntityTypeBuilder<ConfirmEmailToken> builder)
        {
            builder.HasIndex(token => token.Id)
                .IsUnique();

            builder.Property(token => token.Id)
                .HasConversion(
                    id => id.Value,
                    value => new ConfirmEmailTokenId(value));

            builder.Property(token => token.TokenHash)
                .HasMaxLength(TokenHash.MaxLength)
                .HasConversion(
                    tokenHash => tokenHash.Value,
                    value => TokenHash.Create(value).Value);

            builder.Property(token => token.ExpiresAt)
                .HasConversion(
                    timestamp => timestamp.Value,
                    value => Timestamp.Create(value.ToUniversalTime()).Value);

            builder.Ignore(token => token.IsActive);

            builder
                .HasOne(token => token.User)
                .WithMany(user => user.ConfirmEmailTokens)
                .IsRequired();
        }
    }
}
