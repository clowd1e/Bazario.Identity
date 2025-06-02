using Bazario.AspNetCore.Shared.Results;
using Bazario.Identity.Application.Features.Auth.DTO.Responses;
using MediatR;

namespace Bazario.Identity.Application.Features.Auth.Commands.RefreshToken
{
    public sealed record RefreshTokenCommand(
        string AccessToken,
        string RefreshToken,
        Guid SessionId) : IRequest<Result<RefreshTokenResponse>>;
}
