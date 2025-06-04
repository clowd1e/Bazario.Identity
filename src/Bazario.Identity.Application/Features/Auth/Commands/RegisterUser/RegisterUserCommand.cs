using Bazario.AspNetCore.Shared.Abstractions.Messaging;
using Bazario.Identity.Application.Features.Auth.DTO;

namespace Bazario.Identity.Application.Features.Auth.Commands.RegisterUser
{
    public sealed record RegisterUserCommand(
        string Email,
        string Password,
        string RepeatPassword,
        string FirstName,
        string LastName,
        DateOnly BirthDate,
        string PhoneNumber) : IRegisterApplicationUserBaseCommand, ICommand;
}
