using Bazario.AspNetCore.Shared.Abstractions.DomainEvents;
using Bazario.AspNetCore.Shared.Abstractions.MessageBroker;
using Bazario.AspNetCore.Shared.Contracts.UserRegistered;
using Bazario.Identity.Application.Abstractions.Emails;
using Bazario.Identity.Domain.Users.DomainEvents;
using Microsoft.Extensions.Logging;

namespace Bazario.Identity.Application.Features.Auth.Events
{
    internal sealed class UserRegisteredDomainEventHandler
        : IDomainEventHandler<UserRegisteredDomainEvent>
    {
        private readonly ILogger<UserRegisteredDomainEventHandler> _logger;
        private readonly IMessagePublisher _messagePublisher;
        private readonly IEmailLinkGenerator _emailLinkGenerator;

        public UserRegisteredDomainEventHandler(
            ILogger<UserRegisteredDomainEventHandler> logger,
            IMessagePublisher messagePublisher,
            IEmailLinkGenerator emailLinkGenerator)
        {
            _logger = logger;
            _messagePublisher = messagePublisher;
            _emailLinkGenerator = emailLinkGenerator;
        }

        public async Task Handle(
            UserRegisteredDomainEvent domainEvent,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation("Publishing user registered for user service event for user {Id}", domainEvent.UserId);

            await _messagePublisher.PublishAsync<UserRegisteredForUserServiceEvent>(
                new(
                    domainEvent.UserId,
                    domainEvent.Email,
                    domainEvent.FirstName,
                    domainEvent.LastName,
                    domainEvent.BirthDate,
                    domainEvent.PhoneNumber),
                cancellationToken: cancellationToken);

            _logger.LogInformation("Publishing user registered for listing service event for user {Id}", domainEvent.UserId);

            await _messagePublisher.PublishAsync<UserRegisteredForListingServiceEvent>(
                new(
                    domainEvent.UserId,
                    domainEvent.FirstName,
                    domainEvent.LastName,
                    domainEvent.PhoneNumber),
                cancellationToken: cancellationToken);

            _logger.LogInformation("Publishing send confirmation email requested event for user {Id}", domainEvent.UserId);

            var confirmationLink = _emailLinkGenerator.GenerateEmailConfirmationLink(
                domainEvent.UserId,
                domainEvent.ConfirmEmailTokenId,
                domainEvent.ConfirmEmailToken);

            await _messagePublisher.PublishAsync<SendConfirmationEmailRequestedEvent>(
                new(
                    domainEvent.Email,
                    confirmationLink),
                cancellationToken: cancellationToken);
        }
    }
}
