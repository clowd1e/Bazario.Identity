﻿using Bazario.AspNetCore.Shared.Domain.Common.Users;
using Bazario.AspNetCore.Shared.Results;
using Bazario.Identity.Domain.Common.Timestamps;
using Bazario.Identity.Domain.Common.TokenHashes;
using Bazario.Identity.Domain.RefreshTokens.Errors;
using Bazario.Identity.Domain.RefreshTokens.ValueObjects;
using Bazario.Identity.Domain.Users;

namespace Bazario.Identity.Domain.RefreshTokens
{
    public sealed class RefreshToken
    {
        private TokenHash _tokenHash;
        private Timestamp _expiresAt;

        private RefreshToken() { }

        private RefreshToken(
            SessionId sessionId,
            TokenHash tokenHash,
            Timestamp expiresAt,
            User user)
        {
            SessionId = sessionId;
            _tokenHash = tokenHash;
            _expiresAt = expiresAt;
            User = user;
        }

        public UserId UserId { get; private set; }

        public SessionId SessionId { get; private set; }

        public TokenHash TokenHash
        {
            get => _tokenHash;
            private set
            {
                ArgumentNullException.ThrowIfNull(value, nameof(value));
                _tokenHash = value;
            }
        }

        public Timestamp ExpiresAt
        {
            get => _expiresAt;
            private set
            {
                ArgumentNullException.ThrowIfNull(value, nameof(value));
                _expiresAt = value;
            }
        }

        public User? User { get; private set; } = default;

        public Result Populate(
            TokenHash tokenHash,
            Timestamp expiresAt)
        {
            if (expiresAt.Value <= DateTime.UtcNow)
            {
                return Result.Failure(RefreshTokenErrors.InvalidExpiryTime);
            }

            _tokenHash = tokenHash;
            _expiresAt = expiresAt;

            return Result.Success();
        }

        public Result Validate(
            TokenHash tokenHash)
        {
            if (_expiresAt.Value <= DateTime.UtcNow)
            {
                return Result.Failure(RefreshTokenErrors.TokenExpired);
            }

            if (_tokenHash != tokenHash)
            {
                return Result.Failure(RefreshTokenErrors.InvalidToken);
            }

            return Result.Success();
        }

        public static Result<RefreshToken> Create(
            SessionId sessionId,
            TokenHash tokenHash,
            Timestamp expiresAt,
            User user)
        {
            return new RefreshToken(
                sessionId,
                tokenHash,
                expiresAt,
                user);
        }
    }
}
