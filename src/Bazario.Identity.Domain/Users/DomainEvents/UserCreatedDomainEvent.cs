using Bazario.AspNetCore.Shared.Domain;

namespace Bazario.Identity.Domain.Users.DomainEvents
{
    public sealed record UserCreatedDomainEvent(
        string ConfirmEmailToken,
        Guid UserId,
        string Email,
        string FirstName,
        string LastName,
        DateTime BirthDate,
        string PhoneNumber) : DomainEvent;
}
