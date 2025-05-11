using Bazario.AspNetCore.Shared.Options;
using System.ComponentModel.DataAnnotations;

namespace Bazario.Identity.WebAPI.Options
{
    public sealed class OwnerSettings : IAppOptions
    {
        public const string SectionName = nameof(OwnerSettings);

        [Required]
        public string Username { get; init; }

        [EmailAddress]
        [Required]
        public string Email { get; init; }

        [Required]
        public string Password { get; init; }
    }
}
