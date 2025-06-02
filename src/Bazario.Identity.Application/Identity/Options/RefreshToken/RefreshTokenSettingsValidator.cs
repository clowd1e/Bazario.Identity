using Microsoft.Extensions.Options;

namespace Bazario.Identity.Application.Identity.Options.RefreshToken
{
    [OptionsValidator]
    public partial class RefreshTokenSettingsValidator 
        : IValidateOptions<RefreshTokenSettings>
    {
    }
}
