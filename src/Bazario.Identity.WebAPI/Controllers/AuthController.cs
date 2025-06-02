using Bazario.Identity.Application.Features.Auth.Commands.ConfirmEmail;
using Bazario.Identity.Application.Features.Auth.Commands.Login;
using Bazario.Identity.Application.Features.Auth.Commands.RefreshToken;
using Bazario.Identity.Application.Features.Auth.Commands.RegisterAdmin;
using Bazario.Identity.Application.Features.Auth.Commands.RegisterUser;
using Bazario.Identity.Application.Features.Auth.Queries.ValidateEmailConfirmation;
using Bazario.Identity.WebAPI.Factories;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Bazario.Identity.WebAPI.Controllers
{
    [Route("api")]
    [ApiController]
    public sealed class AuthController : ControllerBase
    {
        private readonly ISender _sender;
        private readonly ProblemDetailsFactory _problemDetailsFactory;

        public AuthController(
            ISender sender,
            ProblemDetailsFactory problemDetailsFactory)
        {
            _sender = sender;
            _problemDetailsFactory = problemDetailsFactory;
        }

        #region Queries


        [HttpGet("validate-email-confirmation")]
        public async Task<IActionResult> ValidateEmailConfirmationAsync(
            [FromQuery] Guid userId,
            [FromQuery] Guid tokenId,
            CancellationToken cancellationToken)
        {
            var query = new ValidateEmailConfirmationQuery(userId, tokenId);

            var queryResult = await _sender.Send(query, cancellationToken);

            return queryResult.IsSuccess ? Ok(queryResult.Value) : _problemDetailsFactory.GetProblemDetails(queryResult);
        }


        #endregion

        #region Commands


        [HttpPost("login")]
        public async Task<IActionResult> Login(
            [FromBody] LoginCommand command)
        {
            var commandResult = await _sender.Send(command);

            return commandResult.IsSuccess ? Ok(commandResult.Value) : _problemDetailsFactory.GetProblemDetails(commandResult);
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken(
            [FromBody] RefreshTokenCommand command)
        {
            var commandResult = await _sender.Send(command);

            return commandResult.IsSuccess ? Ok(commandResult.Value) : _problemDetailsFactory.GetProblemDetails(commandResult);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(
            [FromBody] RegisterUserCommand command)
        {
            var commandResult = await _sender.Send(command);

            return commandResult.IsSuccess ? NoContent() : _problemDetailsFactory.GetProblemDetails(commandResult);
        }

        [HttpPost("register-admin")]
        public async Task<IActionResult> RegisterAdmin(
            [FromBody] RegisterAdminCommand command)
        {
            var commandResult = await _sender.Send(command);

            return commandResult.IsSuccess ? NoContent() : _problemDetailsFactory.GetProblemDetails(commandResult);
        }

        [HttpPost("confirm-email")]
        public async Task<IActionResult> ConfirmEmailAsync(
            [FromBody] ConfirmEmailCommand command,
            CancellationToken cancellationToken)
        {
            var commandResult = await _sender.Send(command, cancellationToken);

            return commandResult.IsSuccess ? NoContent() : _problemDetailsFactory.GetProblemDetails(commandResult);
        }


        #endregion
    }
}
