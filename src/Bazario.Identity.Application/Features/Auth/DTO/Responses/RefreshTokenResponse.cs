namespace Bazario.Identity.Application.Features.Auth.DTO.Responses
{
    public sealed record RefreshTokenResponse(
        string AccessToken,
        string RefreshToken);
}
