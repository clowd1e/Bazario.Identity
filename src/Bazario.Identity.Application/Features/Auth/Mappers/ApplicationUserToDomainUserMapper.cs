using Bazario.AspNetCore.Shared.Application.Mappers;
using Bazario.AspNetCore.Shared.Domain.Common.Users;
using Bazario.AspNetCore.Shared.Domain.Common.Users.Emails;
using Bazario.AspNetCore.Shared.Results;
using Bazario.Identity.Application.Identity;
using Bazario.Identity.Domain.Users;

namespace Bazario.Identity.Application.Features.Auth.Mappers
{
    internal sealed class ApplicationUserToDomainUserMapper
        : Mapper<ApplicationUser, Result<User>>
    {
        public override Result<User> Map(ApplicationUser source)
        {
            var userId = new UserId(new Guid(source.Id));

            var emailResult = Email.Create(source.Email);

            if (emailResult.IsFailure)
            {
                return Result.Failure<User>(emailResult.Error);
            }

            var email = emailResult.Value;

            return User.Create(userId, email);
        }
    }
}
