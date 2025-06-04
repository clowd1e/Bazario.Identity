using Bazario.AspNetCore.Shared.Abstractions.Messaging;
using Bazario.Identity.Application.Features.Auth.DTO.Responses;

namespace Bazario.Identity.Application.Features.Auth.Commands.RefreshToken
{
    public sealed record RefreshTokenCommand(
        string AccessToken,
        string RefreshToken,
        Guid SessionId) : ICommand<RefreshTokenResponse>;
}
