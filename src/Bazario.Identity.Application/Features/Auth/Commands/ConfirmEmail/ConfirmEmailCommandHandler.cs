using Bazario.AspNetCore.Shared.Abstractions.Data;
using Bazario.AspNetCore.Shared.Abstractions.Messaging;
using Bazario.AspNetCore.Shared.Domain.Common.Users;
using Bazario.AspNetCore.Shared.Results;
using Bazario.Identity.Application.Abstractions.Identity;
using Bazario.Identity.Application.Abstractions.Security;
using Bazario.Identity.Application.Exceptions;
using Bazario.Identity.Domain.ConfirmEmailTokens;
using Bazario.Identity.Domain.ConfirmEmailTokens.Errors;
using Bazario.Identity.Domain.ConfirmEmailTokens.ValueObjects;
using Bazario.Identity.Domain.Users;
using Bazario.Identity.Domain.Users.Errors;

namespace Bazario.Identity.Application.Features.Auth.Commands.ConfirmEmail
{
    internal sealed class ConfirmEmailCommandHandler
        : ICommandHandler<ConfirmEmailCommand>
    {
        private readonly IConfirmEmailTokenRepository _confirmEmailTokenRepository;
        private readonly IIdentityService _identityService;
        private readonly IUserRepository _userRepository;
        private readonly IHasher _hasher;
        private readonly IUnitOfWork _unitOfWork;

        public ConfirmEmailCommandHandler(
            IConfirmEmailTokenRepository confirmEmailTokenRepository,
            IIdentityService identityService,
            IUserRepository userRepository,
            IHasher hasher,
            IUnitOfWork unitOfWork)
        {
            _confirmEmailTokenRepository = confirmEmailTokenRepository;
            _identityService = identityService;
            _userRepository = userRepository;
            _hasher = hasher;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(
            ConfirmEmailCommand request,
            CancellationToken cancellationToken)
        {
            #region Validate request

            // Check if the user exists

            var userId = new UserId(request.UserId);

            var user = await _userRepository.GetByIdAsync(userId, cancellationToken);

            if (user is null)
            {
                return UserErrors.NotFound;
            }

            // Check if the token exists

            var tokenId = new ConfirmEmailTokenId(request.TokenId);

            var repositoryToken = await _confirmEmailTokenRepository.GetByIdWithUserAsync(tokenId, cancellationToken);

            if (repositoryToken is null)
            {
                return ConfirmEmailTokenErrors.NotFound;
            }

            // Check if user has the token

            if (repositoryToken.User.Id != userId)
            {
                return ConfirmEmailTokenErrors.TokenNotAssignedToUser;
            }

            // Check if the token is already used

            if (repositoryToken.IsUsed)
            {
                return ConfirmEmailTokenErrors.AlreadyUsed;
            }

            // Check if the token is expired

            if (repositoryToken.ExpiresAt.Value < DateTime.UtcNow)
            {
                return ConfirmEmailTokenErrors.Expired;
            }

            #endregion

            // Get identityUser

            var identityUser = await _identityService.GetByEmailAsync(user.Email.Value);

            if (identityUser is null)
            {
                throw new DataInconsistencyException();
            }

            // Check if the user already has an email confirmed

            var isEmailConfirmed = await _identityService.IsEmailConfirmed(identityUser);

            if (isEmailConfirmed)
            {
                return UserErrors.EmailAlreadyConfirmed;
            }

            // Unescape the token

            var unescapedToken = Uri.UnescapeDataString(request.Token);

            // Verify the token

            var tokenHash = _hasher.Hash(unescapedToken);

            if (tokenHash != repositoryToken.TokenHash.Value)
            {
                return ConfirmEmailTokenErrors.Invalid;
            }

            // Use the token

            repositoryToken.Use();

            // Confirm the email in the identity service

            var identityResult = _identityService.ConfirmEmail(identityUser);

            if (identityResult.IsFailure)
            {
                return identityResult.Error;
            }

            // Save changes

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
