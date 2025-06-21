using Bazario.AspNetCore.Shared.Abstractions.Data;
using Bazario.AspNetCore.Shared.Abstractions.Messaging;
using Bazario.AspNetCore.Shared.Application.Mappers;
using Bazario.AspNetCore.Shared.Domain.Common.Users;
using Bazario.AspNetCore.Shared.Domain.Common.Users.Emails;
using Bazario.AspNetCore.Shared.Results;
using Bazario.Identity.Application.Abstractions.Identity;
using Bazario.Identity.Application.Exceptions;
using Bazario.Identity.Application.Features.Auth.DTO.RequestModels;
using Bazario.Identity.Application.Identity;
using Bazario.Identity.Domain.Users;

namespace Bazario.Identity.Application.Features.Auth.Commands.UpdateUser
{
    internal sealed class UpdateUserCommandHandler
        : ICommandHandler<UpdateUserCommand>
    {
        private readonly IIdentityService _identityService;
        private readonly IUserRepository _userRepository;
        private readonly Mapper<UpdateApplicationUserRequestModel, ApplicationUser> _applicationUserMapper;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateUserCommandHandler(
            IIdentityService identityService,
            IUserRepository userRepository,
            Mapper<UpdateApplicationUserRequestModel, ApplicationUser> applicationUserMapper,
            IUnitOfWork unitOfWork)
        {
            _identityService = identityService;
            _userRepository = userRepository;
            _applicationUserMapper = applicationUserMapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(
            UpdateUserCommand command,
            CancellationToken cancellationToken)
        {
            var applicationUser = await _identityService.GetByIdAsync(command.UserId.ToString());

            if (applicationUser is null)
            {
                throw new DataInconsistencyException(
                    "Data inconsistency detected. User updated in external service but doesn't exist in internal.");
            }

            var userId = new UserId(command.UserId);

            var user = await _userRepository.GetByIdAsync(userId, cancellationToken);

            if (user is null)
            {
                throw new DataInconsistencyException();
            }

            var updateIdentityUserRequest = new UpdateApplicationUserRequestModel(
                applicationUser,
                command.Email,
                command.PhoneNumber
            );

            var updatedApplicationUser = _applicationUserMapper.Map(updateIdentityUserRequest);

            await _identityService.UpdateAsync(updatedApplicationUser);

            var emailResult = Email.Create(command.Email);

            if (emailResult.IsFailure)
            {
                throw new NotValidatedDataException($"Couldn't create email of type Email. External email is invalid: {command.Email}");
            }

            var email = emailResult.Value;

            user.Update(email);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
