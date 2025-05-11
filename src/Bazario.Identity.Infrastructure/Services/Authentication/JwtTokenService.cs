using Bazario.AspNetCore.Shared.Authentication.Options;
using Bazario.AspNetCore.Shared.Domain.Common.Users.Roles;
using Bazario.AspNetCore.Shared.Results;
using Bazario.Identity.Application.Abstractions.Identity;
using Bazario.Identity.Application.Identity;
using Bazario.Identity.Domain.Users.Errors;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Bazario.Identity.Infrastructure.Services.Authentication
{
    internal sealed class JwtTokenService : ITokenService
    {
        private readonly JwtSettings _jwtSettings;
        private readonly IIdentityService _identityService;
        private readonly SymmetricSecurityKey _symmetricSecurityKey;

        public JwtTokenService(
            IOptions<JwtSettings> jwtSettings,
            IIdentityService identityService)
        {
            _jwtSettings = jwtSettings.Value;
            _identityService = identityService;
            _symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
        }

        public async Task<string> GenerateAccessTokenAsync(ApplicationUser user)
        {
            var userRole = await _identityService.GetUserRoleAsync(user);

            var claims = GenerateClaims(user, userRole);

            var signingCredentials = new SigningCredentials(
                 key: _symmetricSecurityKey,
                 algorithm: _jwtSettings.SecurityAlgorithm);

            var token = GenerateJwt(claims, signingCredentials);

            return GetJwtTokenString(token);
        }

        public async Task<Result> ValidateAccessTokenAsync(string token, bool validateLifetime = true)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var validationParameters = GetTokenValidationParameters(
                validateLifetime);

            var validationResult = await tokenHandler.ValidateTokenAsync(
                token, validationParameters);

            if (!validationResult.IsValid)
            {
                return Result.Failure(UserErrors.InvalidAccessToken);
            }

            return Result.Success();
        }

        public string GenerateRefreshToken()
        {
            return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        }

        public string GenerateEmailConfirmationToken()
        {
            return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        }

        private JwtSecurityToken GenerateJwt(
            Claim[] claims,
            SigningCredentials credentials)
        {
            return new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims,
                null,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpirationTimeInMinutes),
                signingCredentials: credentials);
        }

        private string GetJwtTokenString(JwtSecurityToken token)
        {
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private Claim[] GenerateClaims(ApplicationUser user, Role role)
        {
            return
            [
                new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new(JwtRegisteredClaimNames.Email, user.Email.ToString()),
                new("role", role.ToString())
            ];
        }

        private TokenValidationParameters GetTokenValidationParameters(
            bool validateLifeTime = true)
        {
            return new()
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = validateLifeTime,
                ValidateIssuerSigningKey = true,
                ValidIssuer = _jwtSettings.Issuer,
                ValidAudience = _jwtSettings.Audience,
                ValidAlgorithms = [_jwtSettings.SecurityAlgorithm],
                ClockSkew = TimeSpan.Zero,
                IssuerSigningKey = _symmetricSecurityKey
            };
        }
    }
}
