using Bazario.AspNetCore.Shared.Domain.Common.Users.Roles;
using Bazario.Identity.Application.Abstractions.Emails;
using Bazario.Identity.Infrastructure.Services.Emails.Options;
using Microsoft.Extensions.Options;
using System.Text;

namespace Bazario.Identity.Infrastructure.Services.Emails
{
    internal sealed class EmailLinkGenerator
        : IEmailLinkGenerator
    {
        private readonly EmailLinkGeneratorSettings _settings;

        public EmailLinkGenerator(
            IOptions<EmailLinkGeneratorSettings> settings)
        {
            _settings = settings.Value;
        }

        public string GenerateEmailConfirmationLink(
            Guid userId,
            Guid tokenId,
            string token,
            Role role)
        {
            var sb = new StringBuilder();

            sb.Append($"{_settings.ClientAppUrl}/");
            
            var confirmationPath = role switch
            {
                Role.User => _settings.EmailConfirmationPath,
                Role.Admin => _settings.AdminEmailConfirmationPath,
                _ => throw new ArgumentOutOfRangeException(nameof(role), role, null)
            };

            sb.Append($"{confirmationPath}?");
            sb.Append($"userId={userId}");
            sb.Append($"&tokenId={tokenId}");
            sb.Append($"&token={token}");

            return sb.ToString();
        }
    }
}
