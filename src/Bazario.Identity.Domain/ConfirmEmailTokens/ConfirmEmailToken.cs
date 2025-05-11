using Bazario.AspNetCore.Shared.Domain;
using Bazario.AspNetCore.Shared.Results;
using Bazario.Identity.Domain.Common.Timestamps;
using Bazario.Identity.Domain.Common.TokenHashes;
using Bazario.Identity.Domain.ConfirmEmailTokens.Errors;
using Bazario.Identity.Domain.ConfirmEmailTokens.ValueObjects;
using Bazario.Identity.Domain.Users;

namespace Bazario.Identity.Domain.ConfirmEmailTokens
{
    public sealed class ConfirmEmailToken
        : AggregateRoot<ConfirmEmailTokenId>
    {
        private TokenHash _tokenHash;
        private Timestamp _expiresAt;

        private ConfirmEmailToken()
            : base(new(Guid.NewGuid())) { }

        public ConfirmEmailToken(
            ConfirmEmailTokenId confirmEmailTokenId,
            TokenHash tokenHash,
            Timestamp expiresAt,
            User user) : base(confirmEmailTokenId)
        {
            TokenHash = tokenHash;
            ExpiresAt = expiresAt;
            User = user;
        }

        public TokenHash TokenHash
        {
            get => _tokenHash;
            private set
            {
                ArgumentNullException.ThrowIfNull(value);
                _tokenHash = value;
            }
        }

        public Timestamp ExpiresAt
        {
            get => _expiresAt;
            private set
            {
                ArgumentNullException.ThrowIfNull(value);
                _expiresAt = value;
            }
        }

        public bool IsActive => !IsUsed && ExpiresAt.Value < DateTime.UtcNow;

        public bool IsUsed { get; private set; }

        public User? User { get; private set; } = default;

        public Result Use()
        {
            if (IsUsed)
            {
                return Result.Failure(
                    ConfirmEmailTokenErrors.AlreadyUsed);
            }

            IsUsed = true;

            return Result.Success();
        }

        public static Result<ConfirmEmailToken> Create(
            ConfirmEmailTokenId confirmEmailTokenId,
            TokenHash tokenHash,
            Timestamp expiresAt,
            User user)
        {
            return new ConfirmEmailToken(
                confirmEmailTokenId,
                tokenHash,
                expiresAt,
                user);
        }
    }
}
