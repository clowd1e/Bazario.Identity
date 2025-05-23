﻿using Bazario.AspNetCore.Shared.Domain;

namespace Bazario.Identity.Domain.Users.DomainEvents
{
    public sealed record UserRegisteredDomainEvent(
        Guid ConfirmEmailTokenId,
        string ConfirmEmailToken,
        Guid UserId,
        string Email,
        string FirstName,
        string LastName,
        DateOnly BirthDate,
        string PhoneNumber) : DomainEvent;
}
