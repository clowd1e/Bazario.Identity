using Bazario.AspNetCore.Shared.Results;
using Bazario.Identity.Application.Identity;

namespace Bazario.Identity.Application.Abstractions.Identity
{
    public interface ITokenService
    {
        Task<string> GenerateAccessTokenAsync(ApplicationUser user);

        string GenerateRefreshToken();

        string GenerateEmailConfirmationToken();

        Task<Result> ValidateAccessTokenAsync(
            string token, bool validateLifetime = true);

        Guid GetUserIdOutOfAccessToken(string accessToken);
    }
}
