using Bazario.AspNetCore.Shared.Domain;

namespace Bazario.Identity.Domain.Users.DomainEvents
{
    public sealed record UserDeletedDomainEvent(
        Guid UserId) : DomainEvent;
}
