using Bazario.AspNetCore.Shared.Abstractions.Data;
using Bazario.AspNetCore.Shared.Abstractions.Messaging;
using Bazario.AspNetCore.Shared.Application.Mappers;
using Bazario.AspNetCore.Shared.Domain.Common.Users;
using Bazario.AspNetCore.Shared.Results;
using Bazario.Identity.Application.Abstractions.Identity;
using Bazario.Identity.Application.Abstractions.Security;
using Bazario.Identity.Application.Exceptions;
using Bazario.Identity.Application.Features.Auth.DTO.RequestModels;
using Bazario.Identity.Application.Features.Auth.DTO.Responses;
using Bazario.Identity.Application.Identity.Options.Login;
using Bazario.Identity.Application.Identity.Options.RefreshToken;
using Bazario.Identity.Domain.Common.Timestamps;
using Bazario.Identity.Domain.Common.TokenHashes;
using Bazario.Identity.Domain.RefreshTokens;
using Bazario.Identity.Domain.RefreshTokens.ValueObjects;
using Bazario.Identity.Domain.Users;
using Bazario.Identity.Domain.Users.Errors;
using Microsoft.Extensions.Options;
using RefreshTokens = Bazario.Identity.Domain.RefreshTokens;

namespace Bazario.Identity.Application.Features.Auth.Commands.Login
{
    internal sealed class LoginCommandHandler
        : ICommandHandler<LoginCommand, LoginResponse>
    {
        private readonly IIdentityService _identityService;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IUserRepository _userRepository;
        private readonly IHasher _hasher;
        private readonly ITokenService _tokenService;
        private readonly LoginSettings _loginSettings;
        private readonly RefreshTokenSettings _refreshTokenSettings;
        private readonly IUnitOfWork _unitOfWork;
        private readonly Mapper<CreateRefreshTokenRequestModel, Result<RefreshTokens.RefreshToken>> _refreshTokenMapper;

        public LoginCommandHandler(
            IIdentityService identityService,
            IRefreshTokenRepository refreshTokenRepository,
            IUserRepository userRepository,
            IHasher hasher,
            ITokenService tokenService,
            IOptions<LoginSettings> loginSettings,
            IOptions<RefreshTokenSettings> refreshTokenSettings,
            IUnitOfWork unitOfWork,
            Mapper<CreateRefreshTokenRequestModel, Result<RefreshTokens.RefreshToken>> refreshTokenMapper)
        {
            _identityService = identityService;
            _refreshTokenRepository = refreshTokenRepository;
            _userRepository = userRepository;
            _hasher = hasher;
            _tokenService = tokenService;
            _loginSettings = loginSettings.Value;
            _refreshTokenSettings = refreshTokenSettings.Value;
            _unitOfWork = unitOfWork;
            _refreshTokenMapper = refreshTokenMapper;
        }

        public async Task<Result<LoginResponse>> Handle(
            LoginCommand request,
            CancellationToken cancellationToken)
        {
            // Login with provided credentials

            var applicationUser = await _identityService.GetByEmailAsync(request.Email);

            if (applicationUser is null)
            {
                return Result.Failure<LoginResponse>(UserErrors.NotFound);
            }

            if (applicationUser.IsBanned)
            {
                return Result.Failure<LoginResponse>(UserErrors.Banned);
            }

            var loginResult = await _identityService.LoginAsync(
                applicationUser,
                password: request.Password);

            if (loginResult.IsFailure)
            {
                return Result.Failure<LoginResponse>(loginResult.Error);
            }

            // Check if email is confirmed

            var isEmailConfirmed = await _identityService.IsEmailConfirmed(applicationUser);

            if (!isEmailConfirmed)
            {
                return Result.Failure<LoginResponse>(UserErrors.EmailNotConfirmed);
            }

            // Get Domain User

            var userId = new UserId(new(applicationUser.Id));

            var user = await _userRepository.GetByIdAsync(userId, cancellationToken);

            if (user is null)
            {
                throw new DataInconsistencyException();
            }

            // Generate access and refresh tokens

            var refreshToken = _tokenService.GenerateRefreshToken();

            var refreshTokenHash = _hasher.Hash(refreshToken);

            var accessToken = await _tokenService.GenerateAccessTokenAsync(applicationUser);

            // Check if refresh token already exists for the session

            var sessionId = new SessionId(request.SessionId);

            var existingRefreshToken = await _refreshTokenRepository.GetBySessionIdWithUserAsync(
                sessionId,
                userId,
                cancellationToken);

            Result? result;

            if (existingRefreshToken is not null)
            {
                result = PopulateRefreshToken(existingRefreshToken, refreshTokenHash);
            }
            else
            {
                result = await InsertRefreshToken(
                    userId,
                    request.SessionId,
                    user,
                    refreshTokenHash,
                    cancellationToken);
            }

            if (result.IsFailure)
            {
                return Result.Failure<LoginResponse>(result.Error);
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new LoginResponse(
                accessToken,
                refreshToken);
        }

        private async Task<Result> InsertRefreshToken(
            UserId userId,
            Guid sessionId,
            User user,
            string refreshTokenHash,
            CancellationToken cancellationToken)
        {

            // Check if user exceeds the maximum number of sessions

            var userSessionsCount = await _refreshTokenRepository.GetUserSessionsCountAsync(
                userId,
                cancellationToken);

            if (userSessionsCount >= _loginSettings.MaxSessionsCount)
            {
                return Result.Failure<LoginResponse>(UserErrors.MaxSessionsExceeded);
            }

            // Insert new refresh token

            var createRefreshTokenRequestModel = new CreateRefreshTokenRequestModel(
                refreshTokenHash,
                user,
                sessionId);

            var tokenMappingResult = _refreshTokenMapper.Map(createRefreshTokenRequestModel);

            if (tokenMappingResult.IsFailure)
            {
                return tokenMappingResult.Error;
            }

            var domainRefreshToken = tokenMappingResult.Value;

            await _refreshTokenRepository.InsertAsync(
                domainRefreshToken,
                cancellationToken);

            return Result.Success();
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
