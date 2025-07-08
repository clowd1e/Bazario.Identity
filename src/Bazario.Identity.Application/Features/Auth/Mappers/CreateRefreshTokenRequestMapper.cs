using Bazario.AspNetCore.Shared.Abstractions.Mappers;
using Bazario.AspNetCore.Shared.Results;
using Bazario.Identity.Application.Features.Auth.DTO.RequestModels;
using Bazario.Identity.Application.Identity.Options.RefreshToken;
using Bazario.Identity.Domain.Common.Timestamps;
using Bazario.Identity.Domain.Common.TokenHashes;
using Bazario.Identity.Domain.RefreshTokens;
using Bazario.Identity.Domain.RefreshTokens.ValueObjects;
using Microsoft.Extensions.Options;

namespace Bazario.Identity.Application.Features.Auth.Mappers
{
    internal sealed class CreateRefreshTokenRequestMapper
        : Mapper<CreateRefreshTokenRequestModel, Result<RefreshToken>>
    {
        private readonly RefreshTokenSettings _settings;

        public CreateRefreshTokenRequestMapper(
            IOptions<RefreshTokenSettings> settings)
        {
            _settings = settings.Value;
        }

        public override Result<RefreshToken> Map(CreateRefreshTokenRequestModel source)
        {
            // Generate token hash

            var tokenHashResult = TokenHash.Create(source.TokenHash);

            if (tokenHashResult.IsFailure)
            {
                return Result.Failure<RefreshToken>(tokenHashResult.Error);
            }

            var tokenHash = tokenHashResult.Value;

            // Generate a new sessionId

            var sessionId = new SessionId(source.SessionId);

            // Calculate expiration timestamp

            var expiresAtResult = Timestamp.Create(
                DateTime.UtcNow.Add(
                    TimeSpan.FromDays(_settings.ExpirationTimeInDays)
                    ));

            if (expiresAtResult.IsFailure)
            {
                return Result.Failure<RefreshToken>(expiresAtResult.Error);
            }

            var expiresAt = expiresAtResult.Value;

            // Create RefreshToken

            var tokenResult = RefreshToken.Create(
                sessionId: sessionId,
                tokenHash: tokenHash,
                expiresAt: expiresAt,
                user: source.User);

            if (tokenResult.IsFailure)
            {
                return Result.Failure<RefreshToken>(tokenResult.Error);
            }

            return tokenResult.Value;
        }
    }
}
