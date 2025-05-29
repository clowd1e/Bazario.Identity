using FluentValidation;

namespace Bazario.Identity.Application.Features.Auth.Commands.RegisterAdmin
{
    internal sealed class RegisterAdminCommandValidator
        : AbstractValidator<RegisterAdminCommand>
    {
        public const string NamePattern = @"^[\p{L}'\-]+$";

        public RegisterAdminCommandValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .Length(5, 50)
                .EmailAddress();

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
