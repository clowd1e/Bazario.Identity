using Bazario.AspNetCore.Shared.Abstractions.Messaging;

namespace Bazario.Identity.Application.Features.Auth.Commands.ConfirmEmail
{
    public sealed record ConfirmEmailCommand(
        Guid UserId,
        Guid TokenId,
        string Token) : ICommand;
}
