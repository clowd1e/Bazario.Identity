using Bazario.AspNetCore.Shared.Results;

namespace Bazario.Identity.Domain.Users.Errors
{
    public static class UserErrors
    {
        public static readonly Error NotFound =
            Error.NotFound(
                code: "User.NotFound",
                description: "User was not found.");

        public static readonly Error EmailNotConfirmed =
            Error.Validation(
                code: "User.EmailNotConfirmed",
                description: "User's email is not confirmed.");

        public static readonly Error InvalidCredentials =
            Error.Validation(
                code: "User.InvalidCredentials",
                description: "The provided credentials are invalid.");

        public static readonly Error InvalidAccessToken =
            Error.Validation(
                code: "User.InvalidAccessToken",
                description: "Invalid access token.");

        public static readonly Error InvalidRefreshToken =
            Error.Validation(
                code: "User.InvalidRefreshToken",
                description: "Invalid refresh token.");

        public static readonly Error RefreshTokenExpired =
            Error.Validation(
                code: "User.RefreshTokenExpired",
                description: "The provided refresh token has expired.");

        public static readonly Error AlreadyRegistered =
            Error.Validation(
                code: "User.AlreadyRegistered",
                description: "User with this email already exists.");

        public static readonly Error EmailAlreadyConfirmed =
            Error.Validation(
                code: "User.EmailAlreadyConfirmed",
                description: "User's email is already confirmed.");

        public static readonly Error MaxSessionsExceeded =
            Error.Validation(
                code: "User.MaxSessionsExceeded",
                description: "The user has exceeded the maximum number of sessions allowed.");

        public static readonly Error Banned =
            Error.Validation(
                code: "User.Banned",
                description: "The user is banned and cannot perform this action.");

        public static Error ChangePasswordFailed(string errors)
            => Error.Validation(
                code: "User.ChangePasswordFailed",
                description: $"Failed to change password. Errors: {errors}");
    }
}
