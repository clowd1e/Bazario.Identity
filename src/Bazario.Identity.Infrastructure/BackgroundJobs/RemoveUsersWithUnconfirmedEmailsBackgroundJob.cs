using Bazario.AspNetCore.Shared.Abstractions.Data;
using Bazario.AspNetCore.Shared.Abstractions.MessageBroker;
using Bazario.AspNetCore.Shared.Domain.Common.Users;
using Bazario.Identity.Application.Abstractions.Identity;
using Bazario.Identity.Application.Identity;
using Bazario.Identity.Domain.Users;
using Microsoft.Extensions.Logging;
using Quartz;

namespace Bazario.Identity.Infrastructure.BackgroundJobs
{
    [DisallowConcurrentExecution]
    internal sealed class RemoveUsersWithUnconfirmedEmailsBackgroundJob : IJob
    {
        private readonly IIdentityService _identityService;
        private readonly IUserRepository _userRepository;
        private readonly IMessagePublisher _messagePublisher;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<RemoveUsersWithUnconfirmedEmailsBackgroundJob> _logger;

        public RemoveUsersWithUnconfirmedEmailsBackgroundJob(
            IIdentityService identityService,
            IUserRepository userRepository,
            IMessagePublisher messagePublisher,
            IUnitOfWork unitOfWork,
            ILogger<RemoveUsersWithUnconfirmedEmailsBackgroundJob> logger)
        {
            _identityService = identityService;
            _userRepository = userRepository;
            _messagePublisher = messagePublisher;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            _logger.LogInformation("Starting removal of users with unconfirmed emails...");

            var usersWithUnconfirmedEmails = await _identityService.GetUsersWithUnconfirmedEmailsAsync();

            if (!usersWithUnconfirmedEmails.Any())
            {
                _logger.LogInformation("No users with unconfirmed emails found.");
                return;
            }

            foreach (var identityUser in usersWithUnconfirmedEmails)
            {
                await DeleteUserAsync(identityUser);
            }

            await _unitOfWork.SaveChangesAsync();
            _logger.LogInformation("Successfully removed {Count} users with unconfirmed emails.", usersWithUnconfirmedEmails.Count());
        }

        private async Task DeleteUserAsync(ApplicationUser identityUser)
        {
            var userId = new UserId(new Guid(identityUser.Id));

            var user = await _userRepository.GetByIdAsync(userId);
            if (user is null)
            {
                _logger.LogWarning("Data Inconsistency. User with ID {UserId} not found in repository, skipping deletion.", userId);
                return;
            }

            if (CheckIfUserHasConfirmationTokens(user))
            {
                return;
            }

            user.Delete();

            await _identityService.DeleteAsync(identityUser);
            await _userRepository.Delete(user);
        }

        private static bool CheckIfUserHasConfirmationTokens(User user)
        {
            return user.ConfirmEmailTokens.Count != 0;
        }
    }
}
