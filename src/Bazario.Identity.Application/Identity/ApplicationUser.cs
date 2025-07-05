using Microsoft.AspNetCore.Identity;

namespace Bazario.Identity.Application.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public bool IsBanned { get; set; }
    }
}
