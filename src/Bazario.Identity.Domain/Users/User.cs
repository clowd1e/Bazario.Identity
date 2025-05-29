using Bazario.AspNetCore.Shared.Domain;
using Bazario.AspNetCore.Shared.Domain.Common.Users;
using Bazario.AspNetCore.Shared.Domain.Common.Users.Emails;
using Bazario.AspNetCore.Shared.Results;
using Bazario.Identity.Domain.ConfirmEmailTokens;
using Bazario.Identity.Domain.Users.DomainEvents;

namespace Bazario.Identity.Domain.Users
{
    public sealed class User
        : AggregateRoot<UserId>
    {
        private HashSet<ConfirmEmailToken> _confirmEmailTokens = [];
        private Email _email;

        private User()
            : base(new(Guid.NewGuid())) { }

        private User(
            UserId userId,
            Email email) : base(userId)
        {
            Email = email;
        }

        public Email Email
        {
            get => _email;
            private set
            {
                ArgumentNullException.ThrowIfNull(value, nameof(value));

                _email = value;
            }
        }

        public IReadOnlyCollection<ConfirmEmailToken> ConfirmEmailTokens => _confirmEmailTokens;

        public void Register(
            Guid confirmEmailTokenId,
            string confirmEmailToken,
            Guid userId,
            string email,
            string firstName,
            string lastName,
            DateOnly birthDate,
            string phoneNumber)
        {
            RaiseDomainEvent(new UserRegisteredDomainEvent(
                confirmEmailTokenId,
                confirmEmailToken,
                userId,
                email,
                firstName,
                lastName,
                birthDate,
                phoneNumber));
        }

        public void RegisterAdmin(
            Guid confirmEmailTokenId,
            string confirmEmailToken,
            Guid userId,
            string email,
            string firstName,
            string lastName,
            DateOnly birthDate,
            string phoneNumber)
        {
            RaiseDomainEvent(new AdminRegisteredDomainEvent(
                confirmEmailTokenId,
                confirmEmailToken,
                userId,
                email,
                firstName,
                lastName,
                birthDate,
                phoneNumber));
        }

        public static Result<User> Create(
            UserId userId,
            Email email)
        {
            return new User(
                userId,
                email);
        }
    }
}
