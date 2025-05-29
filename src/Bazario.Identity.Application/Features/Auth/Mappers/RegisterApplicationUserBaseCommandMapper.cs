using Bazario.AspNetCore.Shared.Application.Mappers;
using Bazario.Identity.Application.Features.Auth.DTO;
using Bazario.Identity.Application.Identity;
using System.Text;

namespace Bazario.Identity.Application.Features.Auth.Mappers
{
    internal sealed class RegisterApplicationUserBaseCommandMapper
        : Mapper<IRegisterApplicationUserBaseCommand, ApplicationUser>
    {
        private readonly char[] forbiddenCharacters = ['\'', '-'];

        public override ApplicationUser Map(IRegisterApplicationUserBaseCommand source)
        {
            var userId = Guid.NewGuid().ToString();

            var username = GenerateUsername(
                source.FirstName,
                source.LastName,
                userId);

            return new ApplicationUser
            {
                Id = userId,
                UserName = username,
                Email = source.Email,
                NormalizedEmail = source.Email.ToUpperInvariant(),
                NormalizedUserName = username.ToUpperInvariant(),
                PhoneNumber = source.PhoneNumber
            };
        }

        private string GenerateUsername(
            string firstName,
            string lastName,
            string userId)
        {
            var sanitizedFirstName = EscapeForbiddenCharacters(firstName);
            var sanitizedLastName = EscapeForbiddenCharacters(lastName);

            var sb = new StringBuilder();

            sb.Append(sanitizedFirstName.ToLowerInvariant());
            sb.Append(sanitizedLastName.ToLowerInvariant());
            sb.Append(userId);

            return sb.ToString();
        }

        private string EscapeForbiddenCharacters(string input)
        {
            var sb = new StringBuilder(input);

            foreach (var character in forbiddenCharacters)
            {
                sb.Replace(character.ToString(), string.Empty);
            }

            return sb.ToString();
        }
    }
}
