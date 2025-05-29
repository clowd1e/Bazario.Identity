using Bazario.AspNetCore.Shared.Abstractions.MessageBroker;
using Bazario.AspNetCore.Shared.Contracts.AdminRegistered;
using Bazario.AspNetCore.Shared.Contracts.UserRegistered;
using Bazario.AspNetCore.Shared.Domain.Common.Users.Roles;
using Bazario.Identity.Application.Abstractions.Emails;
using Bazario.Identity.Domain.Users.DomainEvents;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Bazario.Identity.Application.Features.Auth.Events
{
    internal sealed class AdminRegisteredDomainEventHandler
        : INotificationHandler<AdminRegisteredDomainEvent>
    {
        private readonly ILogger<AdminRegisteredDomainEventHandler> _logger;
        private readonly IMessagePublisher _messagePublisher;
        private readonly IEmailLinkGenerator _emailLinkGenerator;

        public AdminRegisteredDomainEventHandler(IServiceProvider sp)
        {
            _logger = sp.GetRequiredService<ILogger<AdminRegisteredDomainEventHandler>>();

            _messagePublisher = sp.GetRequiredService<IMessagePublisher>();

            _emailLinkGenerator = sp.GetRequiredService<IEmailLinkGenerator>();
        }

        public async Task Handle(
            AdminRegisteredDomainEvent notification,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation("Publishing admin registered for user service event for admin {Id}", notification.UserId);

            await _messagePublisher.PublishAsync<AdminRegisteredForUserServiceEvent>(
                new(
                    notification.UserId,
                    notification.Email,
                    notification.FirstName,
                    notification.LastName,
                    notification.BirthDate,
                    notification.PhoneNumber),
                cancellationToken);

            _logger.LogInformation("Publishing send confirmation email requested event for admin {Id}", notification.UserId);

            var confirmationLink = _emailLinkGenerator.GenerateEmailConfirmationLink(
                notification.UserId,
                notification.ConfirmEmailTokenId,
                notification.ConfirmEmailToken,
                role: Role.Admin);

            await _messagePublisher.PublishAsync<SendConfirmationEmailRequestedEvent>(
                new(
                    notification.Email,
                    confirmationLink),
                cancellationToken);
        }
    }
}
