using Microsoft.Extensions.Options;

namespace Bazario.Identity.Infrastructure.Services.Security.Options
{
    [OptionsValidator]
    public partial class HashSettingsValidator 
        : IValidateOptions<HashSettings>
    {
    }
}
