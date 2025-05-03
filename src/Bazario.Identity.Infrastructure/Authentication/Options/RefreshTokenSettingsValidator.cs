using Microsoft.Extensions.Options;

namespace Bazario.Identity.Infrastructure.Authentication.Options
{
    [OptionsValidator]
    public partial class RefreshTokenSettingsValidator 
        : IValidateOptions<RefreshTokenSettings>
    {
    }
}
