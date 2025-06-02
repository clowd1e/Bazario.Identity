﻿using FluentValidation;

namespace Bazario.Identity.Application.Features.Auth.Commands.RefreshToken
{
    internal sealed class RefreshTokenCommandValidator
        : AbstractValidator<RefreshTokenCommand>
    {
        public RefreshTokenCommandValidator()
        {
            RuleFor(x => x.AccessToken)
                 .NotEmpty()
                 .WithMessage("Access token is required.");

            RuleFor(x => x.RefreshToken)
                .NotEmpty()
                .WithMessage("Refresh token is required.");

            RuleFor(x => x.SessionId)
                .NotEmpty()
                .WithMessage("Session ID is required.");
        }
    }
}
