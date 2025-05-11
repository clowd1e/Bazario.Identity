using Bazario.AspNetCore.Shared.Options;
using System.ComponentModel.DataAnnotations;

namespace Bazario.Identity.Infrastructure.Authentication.Options
{
    public sealed class RefreshTokenSettings : IAppOptions
    {
        public const string SectionName = nameof(RefreshTokenSettings);

        [Required]
        [Range(1, 20)]
        public int ExpiryTimeInDays { get; init; }
    }
}
