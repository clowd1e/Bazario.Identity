using Bazario.AspNetCore.Shared.Contracts.UserRegistered;
using Bazario.AspNetCore.Shared.Infrastructure.MessageBroker;
using Bazario.Identity.Application.Abstractions.Emails;
using Bazario.Identity.Infrastructure.Services.Emails.Options;
using Microsoft.Extensions.Logging;
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

        public string GenerateEmailConfirmationLink(Guid userId, Guid tokenId, string token)
        {
            var sb = new StringBuilder();

            sb.Append($"{_settings.ClientAppUrl}/");
            sb.Append($"{_settings.EmailConfirmationPath}?");
            sb.Append($"userId={userId}");
            sb.Append($"&tokenId={tokenId}");
            sb.Append($"&token={token}");

            return sb.ToString();
        }
    }
}
