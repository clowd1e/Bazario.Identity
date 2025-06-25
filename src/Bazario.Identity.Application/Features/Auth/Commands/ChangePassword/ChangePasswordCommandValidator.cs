using Bazario.Identity.Application.Extensions.Validation;
using FluentValidation;

namespace Bazario.Identity.Application.Features.Auth.Commands.ChangePassword
{
    internal sealed class ChangePasswordCommandValidator
        : AbstractValidator<ChangePasswordCommand>
    {
        public ChangePasswordCommandValidator()
        {
            RuleFor(x => x.OldPassword)
                .NotEmpty();

            RuleFor(x => x.NewPassword)
                .ValidPassword();

            RuleFor(x => x.NewPasswordRepeat)
                .Equal(x => x.NewPassword)
                .WithMessage("New password repeat must match the new password.");
        }
    }
}
