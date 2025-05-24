namespace Bazario.Identity.Application.Abstractions.Emails
{
    public interface IEmailLinkGenerator
    {
        string GenerateEmailConfirmationLink(
            Guid userId,
            Guid tokenId,
            string token);
    }
}
