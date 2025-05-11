using Bazario.Identity.Domain.Users.DomainEvents;
using MediatR;

namespace Bazario.Identity.Application.Features.Auth.Events
{
    internal sealed class UserCreatedDomainEventHandler
    : INotificationHandler<UserCreatedDomainEvent>
    {
        public UserCreatedDomainEventHandler(IServiceProvider sp)
        {
        }

        public async Task Handle(
            UserCreatedDomainEvent notification,
            CancellationToken cancellationToken)
        {
            // TODO: Publish messages to the message broker
        }
    }
}
