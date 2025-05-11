namespace Bazario.Identity.Application.Features.Auth.DTO.Responses
{
    public sealed record LoginResponse(
        string AccessToken,
        string RefreshToken);
}
