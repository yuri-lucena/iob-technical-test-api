using Iob.Bank.Domain.Data.Dtos;
using Iob.Bank.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Iob.Bank.Host.Controllers.Auth
{
    /// <summary>
    /// Controller for authentication-related endpoints.
    /// </summary>
    [Route("api/auth")]
    [ApiController]
    public class AuthController(IAuthService authService) : ControllerBase
    {
        private readonly IAuthService _authService = authService;

        /// <summary>
        /// Signs in a user, returning a JSON Web Token on success.
        /// </summary>
        /// <param name="authRequest">The authentication request.</param>
        /// <returns>A JSON Web Token.</returns>
        /// <response code="200">The user was successfully signed in.</response>
        /// <response code="400">The user could not be signed in.</response>
        [HttpPost("sign-in")]
        [Produces("application/json")]
        [ProducesResponseType<UserAuthResponseDto>(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SignIn([FromBody] UserAuthRequestDto authRequest)
        {
            var response = await _authService.SignInAsync(authRequest);

            return Ok(response);
        }

        /// <summary>
        /// Validates that a JSON Web Token is valid.
        /// </summary>
        /// <returns>True if the token is valid, otherwise false.</returns>
        /// <response code="200">The token is valid.</response>
        [Authorize]
        [HttpPost("verify-token")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(bool), StatusCodes.Status401Unauthorized)]
        public IActionResult VerifyToken()
        {
            return Ok(true);
        }
    }
}
