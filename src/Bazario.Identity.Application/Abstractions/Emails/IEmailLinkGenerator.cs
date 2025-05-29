using Bazario.AspNetCore.Shared.Domain.Common.Users.Roles;

namespace Bazario.Identity.Application.Abstractions.Emails
{
    public interface IEmailLinkGenerator
    {
        string GenerateEmailConfirmationLink(
            Guid userId,
            Guid tokenId,
            string token,
            Role role = Role.User);
    }
}
