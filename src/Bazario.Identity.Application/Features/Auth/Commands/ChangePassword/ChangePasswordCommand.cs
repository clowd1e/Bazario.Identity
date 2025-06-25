using Bazario.AspNetCore.Shared.Abstractions.Messaging;

namespace Bazario.Identity.Application.Features.Auth.Commands.ChangePassword
{
    public sealed record ChangePasswordCommand(
        string OldPassword,
        string NewPassword,
        string NewPasswordRepeat) : ICommand;
}
