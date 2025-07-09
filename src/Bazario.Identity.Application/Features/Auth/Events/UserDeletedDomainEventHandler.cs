using Bazario.AspNetCore.Shared.Abstractions.DomainEvents;
using Bazario.AspNetCore.Shared.Abstractions.MessageBroker;
using Bazario.AspNetCore.Shared.Contracts.UserDeleted;
using Bazario.Identity.Domain.Users.DomainEvents;
using Microsoft.Extensions.Logging;

namespace Bazario.Identity.Application.Features.Auth.Events
{
    internal sealed class UserDeletedDomainEventHandler
        : IDomainEventHandler<UserDeletedDomainEvent>
    {
        private readonly IMessagePublisher _messagePublisher;
        private readonly ILogger<UserDeletedDomainEventHandler> _logger;

        public UserDeletedDomainEventHandler(
            IMessagePublisher messagePublisher,
            ILogger<UserDeletedDomainEventHandler> logger)
        {
            _messagePublisher = messagePublisher;
            _logger = logger;
        }

        public async Task Handle(
            UserDeletedDomainEvent domainEvent,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation("Publishing user deleted event for user {Id}", domainEvent.UserId);

            await _messagePublisher.PublishAsync(
                new UserDeletedEvent(domainEvent.UserId),
                exchangeType: MessageBrokerExchangeType.Fanout,
                cancellationToken: cancellationToken);
        }
    }
}
