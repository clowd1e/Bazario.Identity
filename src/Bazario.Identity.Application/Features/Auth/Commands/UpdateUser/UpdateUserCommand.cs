using Bazario.AspNetCore.Shared.Abstractions.Messaging;

namespace Bazario.Identity.Application.Features.Auth.Commands.UpdateUser
{
    public sealed record UpdateUserCommand(
        Guid UserId,
        string Email,
        string PhoneNumber) : ICommand;
}
