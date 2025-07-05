using Bazario.AspNetCore.Shared.Abstractions.MessageBroker;
using Bazario.AspNetCore.Shared.Abstractions.Messaging;
using Bazario.AspNetCore.Shared.Contracts.UserBanned;
using Bazario.Identity.Application.Features.Auth.Commands.BanUser;
using Microsoft.Extensions.Logging;

namespace Bazario.Identity.Infrastructure.Consumers
{
    internal sealed class UserBannedEventConsumer
        : IMessageConsumer<UserBannedEvent>
    {
        private readonly ICommandHandler<BanUserCommand> _commandHandler;
        private readonly ILogger<UserBannedEventConsumer> _logger;

        public UserBannedEventConsumer(
            ICommandHandler<BanUserCommand> commandHandler,
            ILogger<UserBannedEventConsumer> logger)
        {
            _commandHandler = commandHandler;
            _logger = logger;
        }

        public async Task ConsumeAsync(
            UserBannedEvent message,
            CancellationToken cancellationToken = default)
        {
            _logger.LogInformation(
                "User banned event received. UserId: {UserId}",
                message.UserId);

            var command = new BanUserCommand(message.UserId);

            var result = await _commandHandler.Handle(command, cancellationToken);

            if (result.IsFailure)
            {
                _logger.LogError(
                    "Failed to ban user in Identity Service. UserId: {UserId}, Error: {Error}",
                    message.UserId,
                    result.Error);

                return;
            }

            _logger.LogInformation(
                "User banned successfully. UserId: {UserId}",
                message.UserId);
        }
    }
}
