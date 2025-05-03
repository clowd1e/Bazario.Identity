using Bazario.AspNetCore.Shared.Results;

namespace Bazario.Identity.Domain.Users
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
    }
}
