using Bazario.AspNetCore.Shared.Abstractions.MessageBroker;
using Bazario.AspNetCore.Shared.Abstractions.Messaging;
using Bazario.AspNetCore.Shared.Contracts.UserUpdated;
using Bazario.Identity.Application.Features.Auth.Commands.UpdateUser;
using Microsoft.Extensions.Logging;

namespace Bazario.Identity.Infrastructure.Consumers
{
    internal sealed class UserUpdatedForIdentityServiceEventConsumer
        : IMessageConsumer<UserUpdatedForIdentityServiceEvent>
    {
        private readonly ICommandHandler<UpdateUserCommand> _updateUserCommandHandler;
        private readonly ILogger<UserUpdatedForIdentityServiceEventConsumer> _logger;

        public UserUpdatedForIdentityServiceEventConsumer(
            ICommandHandler<UpdateUserCommand> updateUserCommandHandler,
            ILogger<UserUpdatedForIdentityServiceEventConsumer> logger)
        {
            _updateUserCommandHandler = updateUserCommandHandler;
            _logger = logger;
        }

        public async Task ConsumeAsync(
            UserUpdatedForIdentityServiceEvent message,
            CancellationToken cancellationToken = default)
        {
            _logger.LogInformation(
                "User updated event received. UserId: {UserId}",
                message.UserId);

            var command = new UpdateUserCommand(
                UserId: message.UserId,
                Email: message.Email,
                PhoneNumber: message.PhoneNumber);

            var result = await _updateUserCommandHandler.Handle(command, cancellationToken);

            if (result.IsFailure)
            {
                _logger.LogError(
                    "Failed to update user in Identity Service. UserId: {UserId}, Error: {Error}",
                    message.UserId,
                    result.Error);

                return;
            }

            _logger.LogInformation(
                "User updated successfully. UserId: {UserId}",
                message.UserId);
        }
    }
}
