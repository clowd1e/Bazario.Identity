using Bazario.AspNetCore.Shared.Authorization.Roles;
using Bazario.Identity.Application.Abstractions.Identity;
using Microsoft.AspNetCore.Identity;

namespace Bazario.Identity.Infrastructure.Helpers
{
    public static class RoleSeeder
    {
        public static async Task SeedRolesAsync(IRoleService<IdentityRole> roleManager)
        {
            var roleNames = Enum.GetValues<Role>()
                                .Cast<Role>()
                                .Select(r => r.ToString())
                                .ToArray();

            foreach (var roleName in roleNames)
            {
                if (!await roleManager.ExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }
        }
    }
}
