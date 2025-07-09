using Bazario.AspNetCore.Shared.Options;
using System.ComponentModel.DataAnnotations;

namespace Bazario.Identity.Infrastructure.BackgroundJobs.Options
{
    public sealed class UsersUnconfirmedEmailRemovalSettings : IAppOptions
    {
        public const string SectionName = nameof(UsersUnconfirmedEmailRemovalSettings);

        [Required]
        [Range(0, 23)]
        public int Hours { get; init; }

        [Required]
        [Range(0, 59)]
        public int Minutes { get; init; }

        [Required]
        [Range(1, 31)]
        public int DaysGap { get; init; }
    }
}
