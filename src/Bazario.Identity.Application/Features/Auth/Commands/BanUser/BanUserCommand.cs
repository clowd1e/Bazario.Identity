using Bazario.AspNetCore.Shared.Abstractions.Messaging;

namespace Bazario.Identity.Application.Features.Auth.Commands.BanUser
{
    public sealed record BanUserCommand(
        Guid UserId) : ICommand;
}
