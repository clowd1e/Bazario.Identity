using Bazario.AspNetCore.Shared.Options;
using System.ComponentModel.DataAnnotations;

namespace Bazario.Identity.Infrastructure.BackgroundJobs.Options
{
    public sealed class ExpiredConfirmEmailTokensRemovalSettings
        : IAppOptions
    {
        public const string SectionName = nameof(ExpiredConfirmEmailTokensRemovalSettings);

        [Required]
        [Range(0, 23)]
        public int Hours { get; init; }

        [Required]
        [Range(0, 59)]
        public int Minutes { get; init; }
    }
}
