using SharedJobs = Bazario.AspNetCore.Shared.Infrastructure.BackgroundJobs.DependencyInjection.BackgroundJobsExtensions;
using Bazario.AspNetCore.Shared.Infrastructure.BackgroundJobs.DependencyInjection;
using Bazario.Identity.Infrastructure.Persistence;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Bazario.AspNetCore.Shared.Options;
using Bazario.Identity.Infrastructure.BackgroundJobs.Options;
using Bazario.Identity.Infrastructure.BackgroundJobs;

namespace Bazario.Identity.Infrastructure.Extensions.DI
{
    public static class BackgroundJobsExtensions
    {
        public static IServiceCollection ConfigureAppBackgroundJobs(this IServiceCollection services)
        {
            List<Action<IServiceCollection, IServiceCollectionQuartzConfigurator>> jobConfigurations = [
                SharedJobs.ConfigureProcessOutboxMessagesJob<AppDbContext>,
                ConfigureRemoveExpiredRefreshTokensBackgroundJob];

            services.AddBackgroundJobs([.. jobConfigurations]);

            return services;
        }

        private static void ConfigureRemoveExpiredRefreshTokensBackgroundJob(
            IServiceCollection services,
            IServiceCollectionQuartzConfigurator configurator)
        {
            var settings = services.BuildServiceProvider().GetOptions<ExpiredRefreshTokensRemovalSettings>();

            var jobKey = new JobKey(nameof(RemoveExpiredRefreshTokensBackgroundJob));

            var cronExpression = $"0 {settings.Minutes} {settings.Hours} * * ?";

            configurator
                .AddJob<RemoveExpiredRefreshTokensBackgroundJob>(jobKey)
                .AddTrigger(
                    trigger => trigger.ForJob(jobKey)
                        .WithCronSchedule(cronExpression, builder =>
                            builder.InTimeZone(TimeZoneInfo.Utc)));
        }
    }
}
