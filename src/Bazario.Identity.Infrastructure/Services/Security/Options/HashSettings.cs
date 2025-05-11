using Bazario.AspNetCore.Shared.Options;
using System.ComponentModel.DataAnnotations;

namespace Bazario.Identity.Infrastructure.Services.Security.Options
{
    public sealed class HashSettings : IAppOptions
    {
        public const string SectionName = nameof(HashSettings);

        [Required]
        [MinLength(32)]
        public string SecretKey { get; init; }
    }
}
