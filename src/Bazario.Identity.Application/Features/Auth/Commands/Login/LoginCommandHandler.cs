using Bazario.AspNetCore.Shared.Results;
using Bazario.Identity.Application.Abstractions.Identity;
using Bazario.Identity.Application.Features.Auth.DTO;
using Bazario.Identity.Domain.Users;
using MediatR;

namespace Bazario.Identity.Application.Features.Auth.Commands.Login
{
    internal sealed class LoginCommandHandler
        : IRequestHandler<LoginCommand, Result<LoginResponse>>
    {
        private readonly IIdentityService _identityService;
        private readonly ITokenService _tokenService;

        public LoginCommandHandler(
            IIdentityService identityService,
            ITokenService tokenService)
        {
            _identityService = identityService;
            _tokenService = tokenService;
        }

        public async Task<Result<LoginResponse>> Handle(
            LoginCommand request,
            CancellationToken cancellationToken)
        {
            var user = await _identityService.GetByEmailAsync(request.Email);

            if (user is null)
            {
                return Result.Failure<LoginResponse>(UserErrors.NotFound);
            }

            var loginResult = await _identityService.LoginAsync(
                user,
                password: request.Password);

            if (loginResult.IsFailure)
            {
                return Result.Failure<LoginResponse>(loginResult.Error);
            }

            var isEmailConfirmed = await _identityService.IsEmailConfirmed(user);

            if (!isEmailConfirmed)
            {
                return Result.Failure<LoginResponse>(UserErrors.EmailNotConfirmed);
            }

            var refreshToken = _tokenService.GenerateRefreshToken();

            await _identityService.PopulateRefreshTokenAsync(user, refreshToken);

            var accessToken = await _tokenService.GenerateAccessTokenAsync(user);

            return new LoginResponse(
                accessToken,
                refreshToken);
        }
    }
}
