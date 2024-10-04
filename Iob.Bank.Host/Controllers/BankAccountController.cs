using Iob.Bank.Domain.Data;
using Iob.Bank.Domain.Data.Dtos;
using Iob.Bank.Domain.Interfaces.Services;
using Iob.Bank.Host.Controllers.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Iob.Bank.Host.Controllers;

/// <summary>
/// Controller for managing bank accounts.
/// </summary>
[Route("api/bank-account")]
[ApiController]
public class BankAccountController(IBankAccountService bankAccountService) : BaseController
{
    /// <summary>
    /// Creates a new bank account.
    /// </summary>
    /// <param name="bankAccountDto">The bank account data.</param>
    /// <returns>The created bank account.</returns>
    /// <response code="200">The bank account was created successfully.</response>
    /// <response code="400">The bank account could not be created.</response>
    [Authorize]
    [HttpPost("create")]
    [Produces("application/json")]
    [ProducesResponseType<Response<BankAccountDto>>(StatusCodes.Status200OK)]
    [ProducesResponseType<Response<string>>(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateBankAccount([FromBody] BankAccountDto bankAccountDto)
    {
        var userId = UserId();
        var response = await bankAccountService.CreateAsync(bankAccountDto, userId);

        return Ok(Response<BankAccountDto>.CreateSuccess(response));
    }

    /// <summary>
    /// Updates an existing bank account.
    /// </summary>
    /// <param name="id">The bank account ID.</param>
    /// <param name="bankAccountDto">The updated bank account data.</param>
    /// <returns>The updated bank account.</returns>
    /// <response code="200">The bank account was updated successfully.</response>
    /// <response code="400">The bank account could not be updated.</response>
    /// <response code="404">The bank account could not be found.</response>
    [Authorize]
    [HttpPut("update/{id}")]
    [Produces("application/json")]
    [ProducesResponseType<Response<BankAccountDto>>(StatusCodes.Status200OK)]
    [ProducesResponseType<Response<string>>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<Response<string>>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateBankAccount(long id, [FromBody] BankAccountDto bankAccountDto)
    {
        bankAccountDto.Id = id;
        var userId = UserId();
        var response = await bankAccountService.UpdateAsync(bankAccountDto, userId);

        return Ok(Response<BankAccountDto>.CreateSuccess(response));
    }

    /// <summary>
    /// Deletes an existing bank account.
    /// </summary>
    /// <param name="id">The bank account ID.</param>
    /// <returns>True if the bank account was deleted successfully, otherwise false.</returns>
    /// <response code="200">The bank account was deleted successfully.</response>
    /// <response code="400">The bank account could not be deleted.</response>
    /// <response code="404">The bank account could not be found.</response>
    [Authorize(Roles = "Administrator")]
    [HttpDelete("delete/{id}")]
    [Produces("application/json")]
    [ProducesResponseType<Response<bool>>(StatusCodes.Status200OK)]
    [ProducesResponseType<Response<string>>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<Response<string>>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteBankAccount(long id)
    {
        var userId = UserId();
        var response = await bankAccountService.DeleteAsync(id, userId);

        return Ok(Response<bool>.CreateSuccess(response));
    }

    /// <summary>
    /// Gets a bank account by its ID.
    /// </summary>
    /// <param name="id">The bank account ID.</param>
    /// <returns>The bank account.</returns>
    /// <response code="200">The bank account was found successfully.</response>
    /// <response code="400">The bank account could not be found.</response>
    /// <response code="404">The bank account could not be found.</response>
    [Authorize]
    [HttpGet("get-by/{id}")]
    [Produces("application/json")]
    [ProducesResponseType<Response<BankAccountDto>>(StatusCodes.Status200OK)]
    [ProducesResponseType<Response<string>>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<Response<string>>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetBankAccount(long id)
    {
        var userId = UserId();
        var response = await bankAccountService.GetAsync(id, userId);

        return Ok(Response<BankAccountDto>.CreateSuccess(response));
    }

    /// <summary>
    /// Gets the current balance of a bank account.
    /// </summary>
    /// <param name="bankAccountId">The bank account ID.</param>
    /// <returns>The current balance of the bank account.</returns>
    /// <response code="200">The balance was retrieved successfully.</response>
    /// <response code="400">The balance could not be retrieved.</response>
    /// <response code="404">The bank account could not be found.</response>
    [Authorize]
    [HttpGet("get-account-balance/{bankAccountId:long}")]
    [Produces("application/json")]
    [ProducesResponseType<Response<string>>(StatusCodes.Status200OK)]
    [ProducesResponseType<Response<string>>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<Response<string>>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAccountBalance(long bankAccountId)
    {
        var userId = UserId();
        var response = await bankAccountService.GetAccountBalanceAsync(bankAccountId, userId);

        return Ok(Response<string>.CreateSuccess(response));
    }

    /// <summary>
    /// Gets all bank accounts.
    /// </summary>
    /// <returns>All bank accounts.</returns>
    /// <response code="200">The bank accounts were retrieved successfully.</response>
    /// <response code="400">The bank accounts could not be retrieved.</response>
    [Authorize]
    [HttpGet("get-all")]
    [Produces("application/json")]
    [ProducesResponseType<Response<IEnumerable<BankAccountDto>>>(StatusCodes.Status200OK)]
    [ProducesResponseType<Response<string>>(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAllBankAccounts()
    {
        var userId = UserId();
        var response = await bankAccountService.GetAllAsync(userId);

        return Ok(Response<IEnumerable<BankAccountDto>>.CreateSuccess(response));
    }
}
