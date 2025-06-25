using Bazario.Identity.Application.Extensions.Validation;
using FluentValidation;

namespace Bazario.Identity.Application.Features.Auth.Commands.Login
{
    internal sealed class LoginCommandValidator
        : AbstractValidator<LoginCommand>
    {
        public LoginCommandValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress()
                .Length(5, 50);

            RuleFor(x => x.Password)
                .ValidPassword();

            RuleFor(x => x.SessionId)
                .NotEmpty()
                .WithMessage("Session ID is required.");
        }
    }
}
