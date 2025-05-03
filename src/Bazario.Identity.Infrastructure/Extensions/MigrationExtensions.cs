using Bazario.Identity.Application.Abstractions.Identity;
using Bazario.Identity.Infrastructure.Helpers;
using Bazario.Identity.Infrastructure.Persistence;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Bazario.Identity.Infrastructure.Extensions
{
    public static class MigrationExtensions
    {
        public static void ApplyMigrations(this IApplicationBuilder app)
        {
            using IServiceScope scope = app.ApplicationServices.CreateScope();

            using AppDbContext dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            dbContext.Database.Migrate();
        }

        public async static Task SeedRoles(this IApplicationBuilder app)
        {
            using IServiceScope scope = app.ApplicationServices.CreateScope();

            using AppDbContext dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var roleService = scope.ServiceProvider.GetRequiredService<IRoleService<IdentityRole>>();

            await RoleSeeder.SeedRolesAsync(roleService);
        }
    }
}
