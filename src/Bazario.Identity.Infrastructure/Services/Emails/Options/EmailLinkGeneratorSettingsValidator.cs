using Microsoft.Extensions.Options;

namespace Bazario.Identity.Infrastructure.Services.Emails.Options
{
    [OptionsValidator]
    public partial class EmailLinkGeneratorSettingsValidator
        : IValidateOptions<EmailLinkGeneratorSettings>
    {
    }
}
