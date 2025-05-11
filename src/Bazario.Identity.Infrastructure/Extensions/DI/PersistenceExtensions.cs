using Bazario.AspNetCore.Shared.Application.Abstractions.Data;
using Bazario.AspNetCore.Shared.Infrastructure.Persistence.DependencyInjection;
using Bazario.AspNetCore.Shared.Infrastructure.Persistence.Interceptors;
using Bazario.AspNetCore.Shared.Options;
using Bazario.Identity.Application.Identity;
using Bazario.Identity.Infrastructure.Persistence;
using Bazario.Identity.Infrastructure.Persistence.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Bazario.Identity.Infrastructure.Extensions.DI
{
    internal static class PersistenceExtensions
    {
        public static IServiceCollection AddPersistence(
            this IServiceCollection services)
        {
            services.RegisterInterceptors();

            services.AddAppDbContext();

            services.AddUnitOfWork();

            services.AddAppIdentity();

            return services;
        }

        private static IServiceCollection RegisterInterceptors(
            this IServiceCollection services)
        {
            return services.RegisterInterceptor<ConvertDomainEventsToOutboxMessagesInterceptor>();
        }

        private static void AddAppDbContext(
            this IServiceCollection services)
        {
            services.AddDbContext<AppDbContext>(
                (serviceProvider, options) =>
                {
                    options
                        .UseNpgsqlWithDbSettings(serviceProvider)
                        .AddAppInterceptors(serviceProvider);
                });
        }

        private static DbContextOptionsBuilder UseNpgsqlWithDbSettings(
            this DbContextOptionsBuilder options,
            IServiceProvider serviceProvider)
        {
            var dbSettings = serviceProvider.GetOptions<DbSettings>();

            return options.UseNpgsql(dbSettings.ConnectionString);
        }

        private static DbContextOptionsBuilder AddAppInterceptors(
            this DbContextOptionsBuilder options,
            IServiceProvider serviceProvider)
        {
            var publishDomainEventsInterceptor = serviceProvider
                .GetRequiredService<ConvertDomainEventsToOutboxMessagesInterceptor>();

            return options.AddInterceptors(publishDomainEventsInterceptor);
        }

        private static IServiceCollection AddUnitOfWork(
            this IServiceCollection services)
        {
            return services.AddScoped<IUnitOfWork, UnitOfWork>();
        }

        private static IdentityBuilder AddAppIdentity(
            this IServiceCollection services)
        {
            return services
                .AddIdentity<ApplicationUser, IdentityRole>()
                .AddRoleManager<RoleManager<IdentityRole>>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();
        }
    }
}
