using Bazario.AspNetCore.Shared.Domain.Common.Users;
using Bazario.Identity.Domain.Common.Timestamps;
using Bazario.Identity.Domain.Common.TokenHashes;
using Bazario.Identity.Domain.RefreshTokens;
using Bazario.Identity.Domain.RefreshTokens.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bazario.Identity.Infrastructure.Persistence.Configurations
{
    internal sealed class RefreshTokenConfiguration
        : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.HasKey(token => new { token.UserId, token.SessionId });

            builder.Property(token => token.UserId)
                .HasConversion(
                    userId => userId.Value,
                    value => new UserId(value));

            builder.Property(token => token.SessionId)
                .HasConversion(
                    sessionId => sessionId.Value,
                    value => new SessionId(value));

            builder.Property(token => token.TokenHash)
                .HasMaxLength(TokenHash.MaxLength)
                .HasConversion(
                    tokenHash => tokenHash.Value,
                    value => TokenHash.Create(value).Value);

            builder.Property(token => token.ExpiresAt)
                .HasConversion(
                    timestamp => timestamp.Value,
                    value => Timestamp.Create(value.ToUniversalTime()).Value);

            builder
                .HasOne(token => token.User)
                .WithMany(user => user.RefreshTokens)
                .HasForeignKey(token => token.UserId)
                .IsRequired();
        }
    }
}
