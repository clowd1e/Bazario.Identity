using Bazario.Identity.Domain.Users;

namespace Bazario.Identity.Application.Features.Auth.DTO.RequestModels
{
    public sealed record CreateConfirmEmailTokenRequestModel(
        string TokenHash,
        User User);
}
