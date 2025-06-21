using Bazario.Identity.Application.Identity;

namespace Bazario.Identity.Application.Features.Auth.DTO.RequestModels
{
    public sealed record UpdateApplicationUserRequestModel(
        ApplicationUser ApplicationUser,
        string Email,
        string PhoneNumber);
}
