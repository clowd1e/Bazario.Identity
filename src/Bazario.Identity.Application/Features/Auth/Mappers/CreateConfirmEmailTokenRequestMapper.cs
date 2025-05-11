using Bazario.AspNetCore.Shared.Application.Mappers;
using Bazario.AspNetCore.Shared.Results;
using Bazario.Identity.Application.Features.Auth.DTO.RequestModels;
using Bazario.Identity.Application.Identity.Options;
using Bazario.Identity.Domain.Common.Timestamps;
using Bazario.Identity.Domain.Common.TokenHashes;
using Bazario.Identity.Domain.ConfirmEmailTokens;
using Bazario.Identity.Domain.ConfirmEmailTokens.ValueObjects;
using Microsoft.Extensions.Options;

namespace Bazario.Identity.Application.Features.Auth.Mappers
{
    internal sealed class CreateConfirmEmailTokenRequestMapper
        : Mapper<CreateConfirmEmailTokenRequestModel, Result<ConfirmEmailToken>>
    {
        private readonly ConfirmEmailTokenSettings _settings;

        public CreateConfirmEmailTokenRequestMapper(
            IOptions<ConfirmEmailTokenSettings> settings)
        {
            _settings = settings.Value;
        }

        public override Result<ConfirmEmailToken> Map(
            CreateConfirmEmailTokenRequestModel source)
        {
            // Generate token hash

            var tokenHashResult = TokenHash.Create(source.TokenHash);

            if (tokenHashResult.IsFailure)
            {
                return Result.Failure<ConfirmEmailToken>(tokenHashResult.Error);
            }

            var tokenHash = tokenHashResult.Value;

            // Generate a new ConfirmEmailTokenId

            var tokenId = new ConfirmEmailTokenId(Guid.NewGuid());

            // Generate UTC now timestamp

            var utcNowResult = Timestamp.UtcNow();

            if (utcNowResult.IsFailure)
            {
                return Result.Failure<ConfirmEmailToken>(utcNowResult.Error);
            }

            var utcNow = utcNowResult.Value;

            // Calculate expiration timestamp

            var expiresAtResult = Timestamp.Create(
                utcNow.Value.Add(
                    TimeSpan.FromDays(_settings.ExpirationTimeInDays)
                    ));

            if (expiresAtResult.IsFailure)
            {
                return Result.Failure<ConfirmEmailToken>(expiresAtResult.Error);
            }

            var expiresAt = expiresAtResult.Value;

            // Create ConfirmEmailToken

            var tokenResult = ConfirmEmailToken.Create(
                confirmEmailTokenId: tokenId,
                tokenHash: tokenHash,
                expiresAt: expiresAt,
                user: source.User);

            if (tokenResult.IsFailure)
            {
                return Result.Failure<ConfirmEmailToken>(tokenResult.Error);
            }

            return tokenResult.Value;
        }
    }
}
