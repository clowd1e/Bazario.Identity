using Bazario.Identity.Application.Features.Auth.Commands.Login;
using Bazario.Identity.Application.Features.Auth.Commands.RegisterUser;
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

        #region Commands


        [HttpPost("login")]
        public async Task<IActionResult> Login(
            [FromBody] LoginCommand command)
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

        #endregion
    }
}
