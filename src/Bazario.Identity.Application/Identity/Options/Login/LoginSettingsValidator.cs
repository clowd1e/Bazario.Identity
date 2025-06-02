using Microsoft.Extensions.Options;

namespace Bazario.Identity.Application.Identity.Options.Login
{
    [OptionsValidator]
    public partial class LoginSettingsValidator
        : IValidateOptions<LoginSettings>
    {
    }
}
