using Bazario.AspNetCore.Shared.Domain.Persistence;
using Bazario.AspNetCore.Shared.Infrastructure.Options;
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
            services.AddAppDbContext();

            services.AddUnitOfWork();

            services.AddAppIdentity();

            return services;
        }

        private static void AddAppDbContext(
            this IServiceCollection services)
        {
            services.AddDbContext<AppDbContext>(
                (serviceProvider, options) => options.UseNpgsqlWithDbSettings(serviceProvider));
        }

        private static DbContextOptionsBuilder UseNpgsqlWithDbSettings(
            this DbContextOptionsBuilder options,
            IServiceProvider serviceProvider)
        {
            var dbSettings = serviceProvider.GetOptions<DbSettings>();

            return options.UseNpgsql(dbSettings.ConnectionString);
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
                .AddIdentity<User, IdentityRole>()
                .AddRoleManager<RoleManager<IdentityRole>>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();
        }
    }
}
