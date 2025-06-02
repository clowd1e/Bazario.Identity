using Bazario.AspNetCore.Shared.Options;
using System.ComponentModel.DataAnnotations;

namespace Bazario.Identity.Application.Identity.Options.Login
{
    public sealed class LoginSettings : IAppOptions
    {
        public const string SectionName = nameof(LoginSettings);

        [Required]
        [Range(1, 20)]
        public int MaxSessionsCount { get; init; }
    }
}
