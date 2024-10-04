using FluentValidation;
using Iob.Bank.Domain.Data;
using Iob.Bank.Domain.Data.Dtos;
using Iob.Bank.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Iob.Bank.Host.Controllers
{
    /// <summary>
    /// Controller for user-related operations.
    /// </summary>
    [Route("api/user")]
    [ApiController]
    public class UserController(IUserService userService, IValidator<UserDto> validator) : ControllerBase
    {
        private readonly IUserService _userService = userService;
        private readonly IValidator<UserDto> _validator = validator;

        /// <summary>
        /// Creates a new user.
        /// </summary>
        /// <param name="userDto">The user data.</param>
        /// <returns>The created user.</returns>
        /// <response code="200">The user was created successfully.</response>
        /// <response code="400">The user could not be created.</response>
        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType<Response<UserDto>>(StatusCodes.Status200OK)]
        [ProducesResponseType<Response<string>>(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateUser([FromBody] UserDto userDto)
        {
            var result = _validator.Validate(userDto);
            if (!result.IsValid)
                throw new ValidationException(string.Concat(",", result.Errors.Select(e => e.ErrorMessage)));

            var response = await _userService.CreateAsync(userDto, 1);

            return Ok(Response<UserDto>.CreateSuccess(response));
        }

        /// <summary>
        /// Updates a user.
        /// </summary>
        /// <param name="id">The user ID.</param>
        /// <param name="userDto">The updated user data.</param>
        /// <returns>The updated user.</returns>
        /// <response code="200">The user was updated successfully.</response>
        /// <response code="400">The user could not be updated.</response>
        /// <response code="404">The user could not be found.</response>
        [HttpPut("{id:long}")]
        [Produces("application/json")]
        [ProducesResponseType<Response<UserDto>>(StatusCodes.Status200OK)]
        [ProducesResponseType<Response<string>>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<Response<string>>(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateUser(long id, [FromBody] UserDto userDto)
        {
            userDto.Id = id;

            var result = _validator.Validate(userDto);
            if (!result.IsValid)
                throw new ValidationException(string.Concat(",", result.Errors.Select(e => e.ErrorMessage)));

            var response = await _userService.UpdateAsync(userDto, 1);

            return Ok(Response<UserDto>.CreateSuccess(response));
        }

        /// <summary>
        /// Deletes a user.
        /// </summary>
        /// <param name="id">The user ID.</param>
        /// <returns>True if the user was deleted successfully, otherwise false.</returns>
        /// <response code="200">The user was deleted successfully.</response>
        /// <response code="400">The user could not be deleted.</response>
        /// <response code="404">The user could not be found.</response>
        [HttpDelete("{id:long}")]
        [Produces("application/json")]
        [ProducesResponseType<Response<bool>>(StatusCodes.Status200OK)]
        [ProducesResponseType<Response<string>>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<Response<string>>(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteUser(long id)
        {
            var response = await _userService.DeleteAsync(id, 1);

            return Ok(Response<bool>.CreateSuccess(response));
        }

        /// <summary>
        /// Gets a user by its ID.
        /// </summary>
        /// <param name="id">The user ID.</param>
        /// <returns>The user.</returns>
        /// <response code="200">The user was found successfully.</response>
        /// <response code="400">The user could not be found.</response>
        /// <response code="404">The user could not be found.</response>
        [HttpGet("{id:long}")]
        [Produces("application/json")]
        [ProducesResponseType<Response<UserDto>>(StatusCodes.Status200OK)]
        [ProducesResponseType<Response<string>>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<Response<string>>(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetUser(long id)
        {
            var response = await _userService.GetAsync(id);

            return Ok(Response<UserDto>.CreateSuccess(response));
        }

        /// <summary>
        /// Gets all users.
        /// </summary>
        /// <returns>All users.</returns>
        /// <response code="200">The users were found successfully.</response>
        /// <response code="400">The users could not be found.</response>
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType<Response<IEnumerable<UserDto>>>(StatusCodes.Status200OK)]
        [ProducesResponseType<Response<string>>(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllUsers()
        {
            var response = await _userService.GetAllAsync();

            return Ok(Response<IEnumerable<UserDto>>.CreateSuccess(response));
        }

        /// <summary>
        /// Creates a new user with a bank account.
        /// </summary>
        /// <param name="userDto">The user and bank account data.</param>
        /// <returns>The bank account.</returns>
        /// <response code="200">The user and bank account were created successfully.</response>
        /// <response code="400">The user and bank account could not be created.</response>
        [HttpPost("create-user-with-bank-account")]
        [Produces("application/json")]
        [ProducesResponseType<Response<bool>>(StatusCodes.Status200OK)]
        [ProducesResponseType<Response<string>>(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateUserWithBankAccount([FromBody] UserDto userDto)
        {
            var result = _validator.Validate(userDto);
            if (!result.IsValid)
                throw new ValidationException(string.Concat(",", result.Errors.Select(e => e.ErrorMessage)));

            var response = await _userService.CreateWithBankAccountAsync(userDto);

            return Ok(Response<bool>.CreateSuccess(response));
        }
    }
}
