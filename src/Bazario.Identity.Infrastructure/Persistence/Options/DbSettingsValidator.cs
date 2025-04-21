using Microsoft.Extensions.Options;

namespace Bazario.Identity.Infrastructure.Persistence.Options
{
    [OptionsValidator]
    public partial class DbSettingsValidator 
        : IValidateOptions<DbSettings>
    {
    }
}
