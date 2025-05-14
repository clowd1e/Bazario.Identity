using SharedJobs = Bazario.AspNetCore.Shared.Infrastructure.BackgroundJobs.DependencyInjection.BackgroundJobsExtensions;
using Bazario.AspNetCore.Shared.Infrastructure.BackgroundJobs.DependencyInjection;
using Bazario.Identity.Infrastructure.Persistence;
using Microsoft.Extensions.DependencyInjection;
using Quartz;

namespace Bazario.Identity.Infrastructure.Extensions.DI
{
    public static class BackgroundJobsExtensions
    {
        public static IServiceCollection ConfigureAppBackgroundJobs(this IServiceCollection services)
        {
            List<Action<IServiceCollection, IServiceCollectionQuartzConfigurator>> jobConfigurations = [
                SharedJobs.ConfigureProcessOutboxMessagesJob<AppDbContext>];

            services.AddBackgroundJobs([.. jobConfigurations]);

            return services;
        }
    }
}
