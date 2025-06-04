using Bazario.AspNetCore.Shared.Abstractions.Messaging;
using Bazario.Identity.Application.Features.Auth.Commands.ConfirmEmail;
using Bazario.Identity.Application.Features.Auth.Commands.Login;
using Bazario.Identity.Application.Features.Auth.Commands.RefreshToken;
using Bazario.Identity.Application.Features.Auth.Commands.RegisterAdmin;
using Bazario.Identity.Application.Features.Auth.Commands.RegisterUser;
using Bazario.Identity.Application.Features.Auth.DTO.Responses;
using Bazario.Identity.Application.Features.Auth.Queries.ValidateEmailConfirmation;
using Bazario.Identity.WebAPI.Factories;
using Microsoft.AspNetCore.Mvc;

namespace Bazario.Identity.WebAPI.Controllers
{
    [Route("api")]
    [ApiController]
    public sealed class AuthController(
        #region Handlers
        IQueryHandler<ValidateEmailConfirmationQuery, ValidateEmailConfirmationResponse> validateEmailConfirmationQueryHandler,
        ICommandHandler<LoginCommand, LoginResponse> loginCommandHandler,
        ICommandHandler<RefreshTokenCommand, RefreshTokenResponse> refreshTokenCommandHandler,
        ICommandHandler<RegisterUserCommand> registerUserCommandHandler,
        ICommandHandler<RegisterAdminCommand> registerAdminCommandHandler,
        ICommandHandler<ConfirmEmailCommand> confirmEmailCommandHandler,
        #endregion
        ProblemDetailsFactory problemDetailsFactory) : ControllerBase
    {
        #region Queries


        [HttpGet("validate-email-confirmation")]
        public async Task<IActionResult> ValidateEmailConfirmationAsync(
            [FromQuery] Guid userId,
            [FromQuery] Guid tokenId,
            CancellationToken cancellationToken)
        {
            var query = new ValidateEmailConfirmationQuery(userId, tokenId);

            var queryResult = await validateEmailConfirmationQueryHandler.Handle(query, cancellationToken);

            return queryResult.IsSuccess ? Ok(queryResult.Value) : problemDetailsFactory.GetProblemDetails(queryResult);
        }


        #endregion

        #region Commands


        [HttpPost("login")]
        public async Task<IActionResult> Login(
            [FromBody] LoginCommand command,
            CancellationToken cancellationToken)
        {
            var commandResult = await loginCommandHandler.Handle(command, cancellationToken);

            return commandResult.IsSuccess ? Ok(commandResult.Value) : problemDetailsFactory.GetProblemDetails(commandResult);
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken(
            [FromBody] RefreshTokenCommand command,
            CancellationToken cancellationToken)
        {
            var commandResult = await refreshTokenCommandHandler.Handle(command, cancellationToken);

            return commandResult.IsSuccess ? Ok(commandResult.Value) : problemDetailsFactory.GetProblemDetails(commandResult);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(
            [FromBody] RegisterUserCommand command,
            CancellationToken cancellationToken)
        {
            var commandResult = await registerUserCommandHandler.Handle(command, cancellationToken);

            return commandResult.IsSuccess ? NoContent() : problemDetailsFactory.GetProblemDetails(commandResult);
        }

        [HttpPost("register-admin")]
        public async Task<IActionResult> RegisterAdmin(
            [FromBody] RegisterAdminCommand command,
            CancellationToken cancellationToken)
        {
            var commandResult = await registerAdminCommandHandler.Handle(command, cancellationToken);

            return commandResult.IsSuccess ? NoContent() : problemDetailsFactory.GetProblemDetails(commandResult);
        }

        [HttpPost("confirm-email")]
        public async Task<IActionResult> ConfirmEmailAsync(
            [FromBody] ConfirmEmailCommand command,
            CancellationToken cancellationToken)
        {
            var commandResult = await confirmEmailCommandHandler.Handle(command, cancellationToken);

            return commandResult.IsSuccess ? NoContent() : problemDetailsFactory.GetProblemDetails(commandResult);
        }


        #endregion
    }
}
