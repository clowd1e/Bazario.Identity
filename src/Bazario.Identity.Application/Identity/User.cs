using Microsoft.AspNetCore.Identity;

namespace Bazario.Identity.Application.Identity
{
    public class User : IdentityUser
    {
        public string? RefreshTokenHash { get; set; }

        public DateTime RefreshTokenExpiryTime { get; set; }
    }
}
