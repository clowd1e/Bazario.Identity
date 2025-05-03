using Bazario.Identity.Application.Abstractions.Security;
using Bazario.Identity.Infrastructure.Services.Security.Options;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using System.Text;

namespace Bazario.Identity.Infrastructure.Services.Security
{
    internal sealed class HmacSha256Hasher : IHasher
    {
        private readonly HashSettings _settings;

        public HmacSha256Hasher(
            IOptions<HashSettings> settings)
        {
            _settings = settings.Value;
        }

        public string Hash(string value)
        {
            var secretKeyBytes = Encoding.UTF8.GetBytes(_settings.SecretKey);

            using var hmac = new HMACSHA256(secretKeyBytes);

            var hash = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(value)));

            return hash;
        }
    }
}
