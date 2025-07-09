using Microsoft.Extensions.Options;

namespace Bazario.Identity.Infrastructure.BackgroundJobs.Options
{
    [OptionsValidator]
    public partial class ExpiredConfirmEmailTokensRemovalSettingsValidator
        : IValidateOptions<ExpiredConfirmEmailTokensRemovalSettings>
    {
    }
}
