using Bazario.Identity.Application.Abstractions.Identity;
using Microsoft.AspNetCore.Identity;

namespace Bazario.Identity.Infrastructure.Services.Identity
{
    internal sealed class RoleService<TRole>
        : IRoleService<TRole>
        where TRole : class
    {
        private readonly RoleManager<TRole> _roleManager;

        public RoleService(RoleManager<TRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task CreateAsync(TRole role)
        {
            var result = await _roleManager.CreateAsync(role);

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(x => x.Description);

                var errorsString = string.Join(Environment.NewLine, errors);

                throw new InvalidOperationException($"Failed to create a new role. Errors: {errorsString}");
            }
        }

        public async Task<bool> ExistsAsync(string roleName)
        {
            return await _roleManager.RoleExistsAsync(roleName);
        }
    }
}
