﻿using Bazario.AspNetCore.Shared.Domain.Common.Users.Roles;
using Bazario.AspNetCore.Shared.Results;
using Bazario.Identity.Application.Abstractions.Identity;
using Bazario.Identity.Application.Exceptions;
using Bazario.Identity.Application.Identity;
using Bazario.Identity.Domain.Users.Errors;
using Microsoft.AspNetCore.Identity;

namespace Bazario.Identity.Infrastructure.Services.Identity
{
    internal sealed class IdentityService : IIdentityService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public IdentityService(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task AssignUserToRoleAsync(
            ApplicationUser user,
            string roleName)
        {
            ArgumentNullException.ThrowIfNull(user, nameof(user));
            ArgumentException.ThrowIfNullOrWhiteSpace(roleName, nameof(roleName));

            var result = await _userManager.AddToRoleAsync(user, roleName);

            HandleIdentityResult(result, "Failed to add user to role.");
        }

        public Result ConfirmEmail(
            ApplicationUser user)
        {
            ArgumentNullException.ThrowIfNull(user);

            user.EmailConfirmed = true;

            return Result.Success();
        }

        public async Task CreateAsync(
            ApplicationUser user,
            string password)
        {
            ArgumentNullException.ThrowIfNull(user, nameof(user));
            ArgumentException.ThrowIfNullOrWhiteSpace(password, nameof(password));

            var result = await _userManager.CreateAsync(user, password);

            HandleIdentityResult(result, "Failed to create user.");
        }

        public async Task CreateWithoutPasswordAsync(
            ApplicationUser user)
        {
            ArgumentNullException.ThrowIfNull(user, nameof(user));

            var result = await _userManager.CreateAsync(user);

            HandleIdentityResult(result, "Failed to create user without password.");
        }

        public async Task DeleteAsync(ApplicationUser user)
        {
            ArgumentNullException.ThrowIfNull(user, nameof(user));

            var result = await _userManager.DeleteAsync(user);

            HandleIdentityResult(result, "Failed to delete user.");
        }

        public async Task<ApplicationUser?> GetByEmailAsync(string email)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(email, nameof(email));

            var user = await _userManager.FindByEmailAsync(email);

            return user;
        }

        public async Task<ApplicationUser?> GetByIdAsync(string userId)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(userId, nameof(userId));

            var user = await _userManager.FindByIdAsync(userId);

            return user;
        }

        public async Task<Role> GetUserRoleAsync(ApplicationUser user)
        {
            var userRoles = await _userManager.GetRolesAsync(user);

            if (!userRoles.Any())
            {
                throw new UserNotAssignedToRoleException(user.Id);
            }

            var lastUserRole = userRoles.Last();

            var role = Enum.TryParse<Role>(lastUserRole, out var parsedRole)
                ? parsedRole
                : throw new InvalidOperationException($"Invalid role: {lastUserRole}, for user {user.Id}");

            return role;
        }

        public async Task<bool> IsEmailConfirmed(ApplicationUser user)
        {
            ArgumentNullException.ThrowIfNull(user, nameof(user));

            var isEmailConfirmed = await _userManager.IsEmailConfirmedAsync(user);

            return isEmailConfirmed;
        }

        public async Task<Result> LoginAsync(
            ApplicationUser user,
            string password)
        {
            ArgumentNullException.ThrowIfNull(user, nameof(user));
            ArgumentException.ThrowIfNullOrWhiteSpace(password, nameof(password));

            var result = await _signInManager.PasswordSignInAsync(
                user,
                password,
                true,
                false);

            if (!result.Succeeded)
            {
                return UserErrors.InvalidCredentials;
            }

            return Result.Success();
        }

        private void HandleIdentityResult(IdentityResult result, string errorMessage)
        {
            if (result.Succeeded)
            {
                return;
            }

            var errors = result.Errors.Select(x => x.Description);
            var errorsString = string.Join(Environment.NewLine, errors);

            throw new InvalidOperationException($"{errorMessage}\r\nErrors: {errorsString}");
        }
    }
}
