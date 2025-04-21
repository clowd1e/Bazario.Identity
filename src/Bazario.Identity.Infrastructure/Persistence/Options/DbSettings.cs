using Bazario.AspNetCore.Shared.Infrastructure.Options;
using System.ComponentModel.DataAnnotations;

namespace Bazario.Identity.Infrastructure.Persistence.Options
{
    public class DbSettings : IAppOptions
    {
        public const string SectionName = nameof(DbSettings);

        [Required]
        public required string ConnectionString { get; init; }
    }
}
