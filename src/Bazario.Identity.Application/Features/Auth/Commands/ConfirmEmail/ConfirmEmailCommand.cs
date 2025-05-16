using Bazario.AspNetCore.Shared.Results;
using MediatR;

namespace Bazario.Identity.Application.Features.Auth.Commands.ConfirmEmail
{
    public sealed record ConfirmEmailCommand(
        Guid UserId,
        Guid TokenId,
        string Token) : IRequest<Result>;
}
