using Bazario.AspNetCore.Shared.Abstractions.DomainEvents;
using Bazario.AspNetCore.Shared.Abstractions.MessageBroker;
using Bazario.AspNetCore.Shared.Contracts.AdminRegistered;
using Bazario.AspNetCore.Shared.Contracts.UserRegistered;
using Bazario.AspNetCore.Shared.Domain.Common.Users.Roles;
using Bazario.Identity.Application.Abstractions.Emails;
using Bazario.Identity.Domain.Users.DomainEvents;
using Microsoft.Extensions.Logging;

namespace Bazario.Identity.Application.Features.Auth.Events
{
    internal sealed class AdminRegisteredDomainEventHandler
        : IDomainEventHandler<AdminRegisteredDomainEvent>
    {
        private readonly ILogger<AdminRegisteredDomainEventHandler> _logger;
        private readonly IMessagePublisher _messagePublisher;
        private readonly IEmailLinkGenerator _emailLinkGenerator;

        public AdminRegisteredDomainEventHandler(
            ILogger<AdminRegisteredDomainEventHandler> logger,
            IMessagePublisher messagePublisher,
            IEmailLinkGenerator emailLinkGenerator)
        {
            _logger = logger;
            _messagePublisher = messagePublisher;
            _emailLinkGenerator = emailLinkGenerator;
        }

        public async Task Handle(
            AdminRegisteredDomainEvent domainEvent,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation("Publishing admin registered for user service event for admin {Id}", domainEvent.UserId);

            await _messagePublisher.PublishAsync<AdminRegisteredForUserServiceEvent>(
                new(
                    domainEvent.UserId,
                    domainEvent.Email,
                    domainEvent.FirstName,
                    domainEvent.LastName,
                    domainEvent.BirthDate,
                    domainEvent.PhoneNumber),
                cancellationToken: cancellationToken);

            _logger.LogInformation("Publishing admin registered for complaint service event for admin {Id}", domainEvent.UserId);

            await _messagePublisher.PublishAsync<AdminRegisteredForComplaintServiceEvent>(
                new(
                    domainEvent.UserId,
                    domainEvent.FirstName,
                    domainEvent.LastName),
                cancellationToken: cancellationToken);

            _logger.LogInformation("Publishing send confirmation email requested event for admin {Id}", domainEvent.UserId);

            var confirmationLink = _emailLinkGenerator.GenerateEmailConfirmationLink(
                domainEvent.UserId,
                domainEvent.ConfirmEmailTokenId,
                domainEvent.ConfirmEmailToken,
                role: Role.Admin);

            await _messagePublisher.PublishAsync<SendConfirmationEmailRequestedEvent>(
                new(
                    domainEvent.Email,
                    confirmationLink),
                cancellationToken: cancellationToken);
        }
    }
}
