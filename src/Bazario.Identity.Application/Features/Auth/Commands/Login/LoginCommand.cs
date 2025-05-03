using Bazario.AspNetCore.Shared.Results;
using Bazario.Identity.Application.Features.Auth.DTO;
using MediatR;

namespace Bazario.Identity.Application.Features.Auth.Commands.Login
{
    public sealed record LoginCommand(
        string Email,
        string Password) : IRequest<Result<LoginResponse>>;
}
