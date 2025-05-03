using Microsoft.Extensions.Options;

namespace Bazario.Identity.WebAPI.Options
{
    [OptionsValidator]
    public partial class OwnerSettingsValidator
        : IValidateOptions<OwnerSettings>
    {
    }
}
