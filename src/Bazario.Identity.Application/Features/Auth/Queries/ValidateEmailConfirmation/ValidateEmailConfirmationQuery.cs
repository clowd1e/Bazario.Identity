using Bazario.AspNetCore.Shared.Results;
using Bazario.Identity.Application.Features.Auth.DTO.Responses;
using MediatR;

namespace Bazario.Identity.Application.Features.Auth.Queries.ValidateEmailConfirmation
{
    public sealed record ValidateEmailConfirmationQuery(
        Guid UserId,
        Guid TokenId) : IRequest<Result<ValidateEmailConfirmationResponse>>;
}
