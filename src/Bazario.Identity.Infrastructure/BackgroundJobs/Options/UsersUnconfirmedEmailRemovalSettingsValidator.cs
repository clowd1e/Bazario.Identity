using Microsoft.Extensions.Options;

namespace Bazario.Identity.Infrastructure.BackgroundJobs.Options
{
    [OptionsValidator]
    public partial class UsersUnconfirmedEmailRemovalSettingsValidator
        : IValidateOptions<UsersUnconfirmedEmailRemovalSettings>
    {
    }
}
