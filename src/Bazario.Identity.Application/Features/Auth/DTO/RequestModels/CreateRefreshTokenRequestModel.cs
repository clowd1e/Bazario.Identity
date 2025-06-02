using Bazario.Identity.Domain.Users;

namespace Bazario.Identity.Application.Features.Auth.DTO.RequestModels
{
    public sealed record CreateRefreshTokenRequestModel(
        string TokenHash,
        User User,
        Guid SessionId);
}
