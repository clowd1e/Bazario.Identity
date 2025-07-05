using Bazario.AspNetCore.Shared.Abstractions.Data;
using Bazario.AspNetCore.Shared.Abstractions.Messaging;
using Bazario.AspNetCore.Shared.Results;
using Bazario.Identity.Application.Abstractions.Identity;
using Bazario.Identity.Domain.Users.Errors;

namespace Bazario.Identity.Application.Features.Auth.Commands.BanUser
{
    internal sealed class BanUserCommandHandler
        : ICommandHandler<BanUserCommand>
    {
        private readonly IIdentityService _identityService;
        private readonly IUnitOfWork _unitOfWork;

        public BanUserCommandHandler(
            IIdentityService identityService,
            IUnitOfWork unitOfWork)
        {
            _identityService = identityService;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(
            BanUserCommand request,
            CancellationToken cancellationToken = default)
        {
            var user = await _identityService.GetByIdAsync(
                request.UserId.ToString());

            if (user is null)
            {
                return Result.Failure(UserErrors.NotFound);
            }

            var result = _identityService.BanUserAsync(user);

            if (result.IsFailure)
            {
                return Result.Failure(result.Error);
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
