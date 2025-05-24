using Bazario.AspNetCore.Shared.Abstractions.MessageBroker;
using Bazario.AspNetCore.Shared.Contracts.UserRegistered;
using Bazario.Identity.Application.Abstractions.Emails;
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
        private readonly IMessagePublisher _messagePublisher;
        private readonly IEmailLinkGenerator _emailLinkGenerator;

        public UserRegisteredDomainEventHandler(IServiceProvider sp)
        {
            _logger = sp.GetRequiredService<ILogger<UserRegisteredDomainEventHandler>>();

            _messagePublisher = sp.GetRequiredService<IMessagePublisher>();

            _emailLinkGenerator = sp.GetRequiredService<IEmailLinkGenerator>();
        }

        public async Task Handle(
            UserRegisteredDomainEvent notification,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation("Publishing user registered for user service event for user {Id}", notification.UserId);

            await _messagePublisher.PublishAsync<UserRegisteredForUserServiceEvent>(
                new(
                    notification.UserId,
                    notification.Email,
                    notification.FirstName,
                    notification.LastName,
                    notification.BirthDate,
                    notification.PhoneNumber),
                cancellationToken);

            _logger.LogInformation("Publishing user registered for listing service event for user {Id}", notification.UserId);

            await _messagePublisher.PublishAsync<UserRegisteredForListingServiceEvent>(
                new(
                    notification.UserId,
                    notification.FirstName,
                    notification.LastName,
                    notification.PhoneNumber),
                cancellationToken);

            _logger.LogInformation("Publishing send confirmation email requested event for user {Id}", notification.UserId);

            var confirmationLink = _emailLinkGenerator.GenerateEmailConfirmationLink(
                notification.UserId,
                notification.ConfirmEmailTokenId,
                notification.ConfirmEmailToken);

            await _messagePublisher.PublishAsync<SendConfirmationEmailRequestedEvent>(
                new(
                    notification.Email,
                    confirmationLink),
                cancellationToken);
        }
    }
}
