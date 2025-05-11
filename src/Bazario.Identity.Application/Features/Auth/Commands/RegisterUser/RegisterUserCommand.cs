using Bazario.AspNetCore.Shared.Results;
using MediatR;

namespace Bazario.Identity.Application.Features.Auth.Commands.RegisterUser
{
    public sealed record RegisterUserCommand(
        string Email,
        string Password,
        string RepeatPassword,
        string FirstName,
        string LastName,
        DateTime BirthDate,
        string PhoneNumber) : IRequest<Result>;
}
