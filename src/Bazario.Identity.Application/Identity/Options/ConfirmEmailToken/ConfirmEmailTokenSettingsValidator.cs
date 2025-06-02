using Bazario.Identity.Application.Identity.Options.ConfirmEmailToken;
using Microsoft.Extensions.Options;

namespace Bazario.Identity.Application.Identity.Options
{
    [OptionsValidator]
    public partial class ConfirmEmailTokenSettingsValidator
        : IValidateOptions<ConfirmEmailTokenSettings>
    {
    }
}
