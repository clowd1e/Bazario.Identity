using Bazario.Identity.Application.Extensions.Validation;
using FluentValidation;

namespace Bazario.Identity.Application.Features.Auth.Commands.RegisterUser
{
    internal sealed class RegisterUserCommandValidator
        : AbstractValidator<RegisterUserCommand>
    {
        public const string NamePattern = @"^[\p{L}'\-]+$";

        public RegisterUserCommandValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .Length(5, 50)
                .EmailAddress();

            RuleFor(x => x.Password)
                .ValidPassword();

            RuleFor(x => x.RepeatPassword)
                .NotEmpty()
                .Equal(x => x.Password);

            RuleFor(x => x.FirstName)
                .NotEmpty()
                .Length(2, 30)
                .Matches(NamePattern);

            RuleFor(x => x.LastName)
                .NotEmpty()
                .Length(2, 30)
                .Matches(NamePattern);

            RuleFor(x => x.BirthDate)
                .NotEmpty()
                .LessThan(DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-18)));

            RuleFor(x => x.PhoneNumber)
                .NotEmpty()
                .Length(10, 15)
                .Matches(@"^\+?[0-9\s\-\(\)]{7,20}$");
        }
    }
}
