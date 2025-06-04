using Bazario.AspNetCore.Shared.Abstractions.Messaging;
using Bazario.Identity.Application.Features.Auth.DTO.Responses;

namespace Bazario.Identity.Application.Features.Auth.Queries.ValidateEmailConfirmation
{
    public sealed record ValidateEmailConfirmationQuery(
        Guid UserId,
        Guid TokenId) : IQuery<ValidateEmailConfirmationResponse>;
}
