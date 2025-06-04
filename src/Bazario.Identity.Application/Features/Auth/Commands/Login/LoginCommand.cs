using Bazario.AspNetCore.Shared.Abstractions.Messaging;
using Bazario.AspNetCore.Shared.Results;
using Bazario.Identity.Application.Features.Auth.DTO.Responses;

namespace Bazario.Identity.Application.Features.Auth.Commands.Login
{
    public sealed record LoginCommand(
        string Email,
        string Password,
        Guid SessionId) : ICommand<LoginResponse>;
}
