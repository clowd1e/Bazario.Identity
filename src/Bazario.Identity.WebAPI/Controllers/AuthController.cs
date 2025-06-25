using Bazario.AspNetCore.Shared.Abstractions.Messaging;
using Bazario.AspNetCore.Shared.Api.Factories;
using Bazario.Identity.Application.Features.Auth.Commands.ChangePassword;
using Bazario.Identity.Application.Features.Auth.Commands.ConfirmEmail;
using Bazario.Identity.Application.Features.Auth.Commands.Login;
using Bazario.Identity.Application.Features.Auth.Commands.RefreshToken;
using Bazario.Identity.Application.Features.Auth.Commands.RegisterAdmin;
using Bazario.Identity.Application.Features.Auth.Commands.RegisterUser;
using Bazario.Identity.Application.Features.Auth.DTO.Responses;
using Bazario.Identity.Application.Features.Auth.Queries.ValidateEmailConfirmation;
using Microsoft.AspNetCore.Mvc;

namespace Bazario.Identity.WebAPI.Controllers
{
    [Route("api")]
    [ApiController]
    public sealed class AuthController(
        ProblemDetailsFactory problemDetailsFactory) : ControllerBase
    {
        #region Queries


        [HttpGet("validate-email-confirmation")]
        public async Task<IActionResult> ValidateEmailConfirmationAsync(
            [FromServices] IQueryHandler<ValidateEmailConfirmationQuery, ValidateEmailConfirmationResponse> queryHandler,
            [FromQuery] Guid userId,
            [FromQuery] Guid tokenId,
            CancellationToken cancellationToken)
        {
            var query = new ValidateEmailConfirmationQuery(userId, tokenId);

            var queryResult = await queryHandler.Handle(query, cancellationToken);

            return queryResult.IsSuccess ? Ok(queryResult.Value) : problemDetailsFactory.GetProblemDetails(queryResult);
        }


        #endregion

        #region Commands


        [HttpPost("login")]
        public async Task<IActionResult> Login(
            [FromServices] ICommandHandler<LoginCommand, LoginResponse> commandHandler,
            [FromBody] LoginCommand command,
            CancellationToken cancellationToken)
        {
            var commandResult = await commandHandler.Handle(command, cancellationToken);

            return commandResult.IsSuccess ? Ok(commandResult.Value) : problemDetailsFactory.GetProblemDetails(commandResult);
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken(
            [FromServices] ICommandHandler<RefreshTokenCommand, RefreshTokenResponse> commandHandler,
            [FromBody] RefreshTokenCommand command,
            CancellationToken cancellationToken)
        {
            var commandResult = await commandHandler.Handle(command, cancellationToken);

            return commandResult.IsSuccess ? Ok(commandResult.Value) : problemDetailsFactory.GetProblemDetails(commandResult);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(
            [FromServices] ICommandHandler<RegisterUserCommand> commandHandler,
            [FromBody] RegisterUserCommand command,
            CancellationToken cancellationToken)
        {
            var commandResult = await commandHandler.Handle(command, cancellationToken);

            return commandResult.IsSuccess ? NoContent() : problemDetailsFactory.GetProblemDetails(commandResult);
        }

        [HttpPost("register-admin")]
        public async Task<IActionResult> RegisterAdmin(
            [FromServices] ICommandHandler<RegisterAdminCommand> commandHandler,
            [FromBody] RegisterAdminCommand command,
            CancellationToken cancellationToken)
        {
            var commandResult = await commandHandler.Handle(command, cancellationToken);

            return commandResult.IsSuccess ? NoContent() : problemDetailsFactory.GetProblemDetails(commandResult);
        }

        [HttpPost("confirm-email")]
        public async Task<IActionResult> ConfirmEmailAsync(
            [FromServices] ICommandHandler<ConfirmEmailCommand> commandHandler,
            [FromBody] ConfirmEmailCommand command,
            CancellationToken cancellationToken)
        {
            var commandResult = await commandHandler.Handle(command, cancellationToken);

            return commandResult.IsSuccess ? NoContent() : problemDetailsFactory.GetProblemDetails(commandResult);
        }

        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePasswordAsync(
            [FromServices] ICommandHandler<ChangePasswordCommand> commandHandler,
            [FromBody] ChangePasswordCommand command,
            CancellationToken cancellationToken)
        {
            var commandResult = await commandHandler.Handle(command, cancellationToken);

            return commandResult.IsSuccess ? NoContent() : problemDetailsFactory.GetProblemDetails(commandResult);
        }


        #endregion
    }
}
