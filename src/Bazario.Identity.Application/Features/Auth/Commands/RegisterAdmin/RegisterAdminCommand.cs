using Bazario.AspNetCore.Shared.Abstractions.Messaging;
using Bazario.Identity.Application.Features.Auth.DTO;

namespace Bazario.Identity.Application.Features.Auth.Commands.RegisterAdmin
{
    public sealed record RegisterAdminCommand(
        string Email,
        string FirstName,
        string LastName,
        DateOnly BirthDate,
        string PhoneNumber) : IRegisterApplicationUserBaseCommand, ICommand;
}
