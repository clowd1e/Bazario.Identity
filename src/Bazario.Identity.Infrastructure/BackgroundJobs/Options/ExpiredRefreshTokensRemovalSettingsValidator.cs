using Microsoft.Extensions.Options;

namespace Bazario.Identity.Infrastructure.BackgroundJobs.Options
{
    [OptionsValidator]
    public partial class ExpiredRefreshTokensRemovalSettingsValidator
        : IValidateOptions<ExpiredRefreshTokensRemovalSettings>
    {
    }
}
