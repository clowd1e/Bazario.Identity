using Bazario.AspNetCore.Shared.Abstractions.Data;
using Bazario.AspNetCore.Shared.Abstractions.Messaging;
using Bazario.AspNetCore.Shared.Domain.Common.Users;
using Bazario.AspNetCore.Shared.Results;
using Bazario.Identity.Application.Abstractions.Identity;
using Bazario.Identity.Application.Abstractions.Security;
using Bazario.Identity.Application.Features.Auth.DTO.Responses;
using Bazario.Identity.Application.Identity.Options.RefreshToken;
using Bazario.Identity.Domain.Common.Timestamps;
using Bazario.Identity.Domain.Common.TokenHashes;
using Bazario.Identity.Domain.RefreshTokens;
using Bazario.Identity.Domain.RefreshTokens.Errors;
using Bazario.Identity.Domain.RefreshTokens.ValueObjects;
using Bazario.Identity.Domain.Users.Errors;
using Microsoft.Extensions.Options;
using RefreshTokens = Bazario.Identity.Domain.RefreshTokens;

namespace Bazario.Identity.Application.Features.Auth.Commands.RefreshToken
{
    internal sealed class RefreshTokenCommandHandler
        : ICommandHandler<RefreshTokenCommand, RefreshTokenResponse>
    {
        private readonly ITokenService _tokenService;
        private readonly IHasher _hasher;
        private readonly IIdentityService _identityService;
        private readonly RefreshTokenSettings _refreshTokenSettings;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IUnitOfWork _unitOfWork;

        public RefreshTokenCommandHandler(
            ITokenService tokenService,
            IHasher hasher,
            IIdentityService identityService,
            IOptions<RefreshTokenSettings> refreshTokenSettings,
            IRefreshTokenRepository refreshTokenRepository,
            IUnitOfWork unitOfWork)
        {
            _tokenService = tokenService;
            _hasher = hasher;
            _identityService = identityService;
            _refreshTokenSettings = refreshTokenSettings.Value;
            _refreshTokenRepository = refreshTokenRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<RefreshTokenResponse>> Handle(
            RefreshTokenCommand request,
            CancellationToken cancellationToken)
        {
            // Validate the access token

            var accessValidationResult = await _tokenService.ValidateAccessTokenAsync(
                request.AccessToken, false);

            if (accessValidationResult.IsFailure)
            {
                return Result.Failure<RefreshTokenResponse>(accessValidationResult.Error);
            }

            var userId = new UserId(_tokenService.GetUserIdOutOfAccessToken(request.AccessToken));

            // Get refresh token by session ID

            var sessionId = new SessionId(request.SessionId);

            var refreshToken = await _refreshTokenRepository.GetBySessionIdWithUserAsync(
                sessionId,
                userId,
                cancellationToken);

            if (refreshToken is null)
            {
                return Result.Failure<RefreshTokenResponse>(RefreshTokenErrors.NotFound);
            }

            // Get identity user

            var applicationUser = await _identityService
                .GetByIdAsync(refreshToken.UserId.Value.ToString());

            if (applicationUser is null)
            {
                return Result.Failure<RefreshTokenResponse>(UserErrors.NotFound);
            }

            // Hash the refresh token

            var refreshTokenHash = _hasher.Hash(request.RefreshToken);

            var tokenHashResult = TokenHash.Create(refreshTokenHash);

            if (tokenHashResult.IsFailure)
            {
                return Result.Failure<RefreshTokenResponse>(tokenHashResult.Error);
            }

            var tokenHash = tokenHashResult.Value;

            // Validate the refresh token

            var validationResult = refreshToken.Validate(
                tokenHash);

            if (validationResult.IsFailure)
            {
                return Result.Failure<RefreshTokenResponse>(validationResult.Error);
            }

            // Populate the refresh token

            var newRefreshToken = _tokenService.GenerateRefreshToken();

            var newRefreshTokenHash = _hasher.Hash(newRefreshToken);

            var populateRefreshTokenResult = PopulateRefreshToken(
                refreshToken,
                newRefreshTokenHash);

            if (populateRefreshTokenResult.IsFailure)
            {
                return Result.Failure<RefreshTokenResponse>(populateRefreshTokenResult.Error);
            }

            // Generate new access token

            var accessToken = await _tokenService.GenerateAccessTokenAsync(applicationUser);

            // Save the updated refresh token

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success(
                new RefreshTokenResponse(accessToken, newRefreshToken));
        }

        private Result PopulateRefreshToken(
            RefreshTokens.RefreshToken domainRefreshToken,
            string newTokenHash)
        {
            var tokenHashResult = TokenHash.Create(newTokenHash);

            if (tokenHashResult.IsFailure)
            {
                return tokenHashResult.Error;
            }

            var tokenHash = tokenHashResult.Value;

            var expiresAtResult = Timestamp.Create(
                DateTime.UtcNow.Add(
                    TimeSpan.FromDays(
                        _refreshTokenSettings.ExpirationTimeInDays)));

            if (expiresAtResult.IsFailure)
            {
                return expiresAtResult.Error;
            }

            var expiresAt = expiresAtResult.Value;

            domainRefreshToken.Populate(tokenHash, expiresAt);

            return Result.Success();
        }
    }
}
