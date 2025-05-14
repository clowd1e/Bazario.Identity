using Bazario.AspNetCore.Shared.Application.Abstractions.EventBus;
using Bazario.AspNetCore.Shared.Contracts.UserRegistered;
using Bazario.Identity.Domain.Users.DomainEvents;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Bazario.Identity.Application.Features.Auth.Events
{
    internal sealed class UserRegisteredDomainEventHandler
    : INotificationHandler<UserRegisteredDomainEvent>
    {
        private readonly ILogger<UserRegisteredDomainEventHandler> _logger;
        private readonly IEventBus _eventBus;

        public UserRegisteredDomainEventHandler(IServiceProvider sp)
        {
            _logger = sp.GetRequiredService<ILogger<UserRegisteredDomainEventHandler>>();

            _eventBus = sp.GetRequiredService<IEventBus>();
        }

        public async Task Handle(
            UserRegisteredDomainEvent notification,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation("Publishing user registered for user service event for user {Id}", notification.UserId);

            await _eventBus.PublishAsync<UserRegisteredForUserServiceEvent>(
                new (
                    notification.UserId,
                    notification.Email,
                    notification.FirstName,
                    notification.LastName,
                    notification.BirthDate,
                    notification.PhoneNumber),
                cancellationToken);

            _logger.LogInformation("Publishing user registered for listing service event for user {Id}", notification.UserId);

            await _eventBus.PublishAsync<UserRegisteredForListingServiceEvent>(
                new(
                    notification.UserId,
                    notification.FirstName,
                    notification.LastName,
                    notification.PhoneNumber),
                cancellationToken);

            _logger.LogInformation("Publishing send confirmation email requested event for user {Id}", notification.UserId);

            await _eventBus.PublishAsync<SendConfirmationEmailRequestedEvent>(
                new(
                    notification.UserId,
                    notification.Email,
                    notification.ConfirmEmailTokenId,
                    notification.ConfirmEmailToken),
                cancellationToken);
        }
    }
}
