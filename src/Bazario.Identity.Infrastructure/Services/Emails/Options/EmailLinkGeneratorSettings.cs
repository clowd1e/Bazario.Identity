using Bazario.AspNetCore.Shared.Options;
using System.ComponentModel.DataAnnotations;

namespace Bazario.Identity.Infrastructure.Services.Emails.Options
{
    public sealed class EmailLinkGeneratorSettings : IAppOptions
    {
        public const string SectionName = nameof(EmailLinkGeneratorSettings);

        [Required]
        [StringLength(100)]
        [Url]
        public required string ClientAppUrl { get; set; }

        [Required]
        [StringLength(100)]
        public required string EmailConfirmationPath { get; set; }
    }
}
