using Metrica.Authentication.Api.Dtos.Auth;
using Metrica.Authentication.Application.Commands.Auth.Login;
using Microsoft.AspNetCore.Mvc;

namespace Metrica.Authentication.Api.Controllers
{
    [ApiController]
    [Route("auth")]
    public sealed class AuthController : ControllerBase
    {
        private readonly LoginCommandHandler _loginCommandHandler;

        public AuthController(LoginCommandHandler loginCommandHandler)
        {
            _loginCommandHandler = loginCommandHandler;
        }

        [HttpPost("login")]
        [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<LoginResponse>> LoginAsync(
            [FromBody] LoginRequest request,
            CancellationToken cancellationToken)
        {
            var result = await _loginCommandHandler.HandleAsync(
                new LoginCommand(request.Email, request.Password),
                cancellationToken);

            if (result is null)
            {
                return Unauthorized(new
                {
                    message = "Las credenciales no son válidas."
                });
            }

            return Ok(new LoginResponse(
                result.AccessToken,
                "Bearer",
                result.ExpiresAtUtc));
        }
    }
}