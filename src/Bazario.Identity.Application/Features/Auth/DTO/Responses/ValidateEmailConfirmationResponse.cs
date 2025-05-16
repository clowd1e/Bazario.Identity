namespace Bazario.Identity.Application.Features.Auth.DTO.Responses
{
    public sealed record ValidateEmailConfirmationResponse(
        DateTime ExpiresAt);
}
