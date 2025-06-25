using Bazario.AspNetCore.Shared.Abstractions;
using Bazario.AspNetCore.Shared.Abstractions.Messaging;
using Bazario.AspNetCore.Shared.Results;
using Bazario.Identity.Application.Abstractions.Identity;
using Bazario.Identity.Domain.Users.Errors;

namespace Bazario.Identity.Application.Features.Auth.Commands.ChangePassword
{
    internal sealed class ChangePasswordCommandHandler
        : ICommandHandler<ChangePasswordCommand>
    {
        private readonly IUserContextService _userContextService;
        private readonly IIdentityService _identityService;

        public ChangePasswordCommandHandler(
            IUserContextService userContextService,
            IIdentityService identityService)
        {
            _userContextService = userContextService;
            _identityService = identityService;
        }

        public async Task<Result> Handle(
            ChangePasswordCommand command,
            CancellationToken cancellationToken)
        {
            var userId = _userContextService.GetAuthenticatedUserId();

            var user = await _identityService.GetByIdAsync(userId.ToString());

            if (user is null)
            {
                return UserErrors.NotFound;
            }

            var changePasswordResult = await _identityService.ChangePassword(
                user,
                command.OldPassword,
                command.NewPassword);

            if (changePasswordResult.IsFailure)
            {
                return changePasswordResult.Error;
            }

            return Result.Success();
        }
    }
}
