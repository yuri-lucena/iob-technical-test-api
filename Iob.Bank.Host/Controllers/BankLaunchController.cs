using Iob.Bank.Domain.Data;
using Iob.Bank.Domain.Data.Dtos;
using Iob.Bank.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Iob.Bank.Host.Controllers;

/// <summary>
/// Controller for managing bank launches.
/// </summary>
[Route("api/bank-launch")]
[ApiController]
public class LaunchBankController(IBankLaunchService bankLaunchService) : ControllerBase
{
    /// <summary>
    /// Creates a new credit bank launch.
    /// </summary>
    /// <param name="bankLaunchDto">The credit bank launch data.</param>
    /// <returns>A boolean indicating whether the credit bank launch was created successfully.</returns>
    /// <response code="200">The credit bank launch was created successfully.</response>
    /// <response code="400">The credit bank launch could not be created.</response>
    /// <response code="404">The bank account could not be found.</response>
    [HttpPost("credit")]
    [Produces("application/json")]
    [ProducesResponseType<Response<bool>>(StatusCodes.Status200OK)]
    [ProducesResponseType<Response<string>>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<Response<string>>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CreateLaunchCredit([FromBody] BankLaunchDto bankLaunchDto)
    {
        var response = await bankLaunchService.CreateCreditLaunchAsync(bankLaunchDto, 1);

        return Ok(Response<bool>.CreateSuccess(response));
    }

    /// <summary>
    /// Creates a new debit bank launch.
    /// </summary>
    /// <param name="bankLaunchDto">The debit bank launch data.</param>
    /// <returns>A boolean indicating whether the debit bank launch was created successfully.</returns>
    /// <response code="200">The debit bank launch was created successfully.</response>
    /// <response code="400">The debit bank launch could not be created.</response>
    /// <response code="404">The bank account could not be found.</response>
    [HttpPost("debit")]
    [Produces("application/json")]
    [ProducesResponseType<Response<bool>>(StatusCodes.Status200OK)]
    [ProducesResponseType<Response<string>>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<Response<string>>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CreateLaunch([FromBody] BankLaunchDto bankLaunchDto)
    {
        var response = await bankLaunchService.CreateDebitLaunchAsync(bankLaunchDto, 1);

        return Ok(Response<bool>.CreateSuccess(response));
    }

    /// <summary>
    /// Creates a new transfer bank launch.
    /// </summary>
    /// <param name="bankLaunchDto">The transfer bank launch data.</param>
    /// <returns>A boolean indicating whether the transfer bank launch was created successfully.</returns>
    /// <response code="200">The transfer bank launch was created successfully.</response>
    /// <response code="400">The transfer bank launch could not be created.</response>
    /// <response code="404">The bank account could not be found.</response>
    [HttpPost("transfer")]
    [Produces("application/json")]
    [ProducesResponseType<Response<bool>>(StatusCodes.Status200OK)]
    [ProducesResponseType<Response<string>>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<Response<string>>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CreateLaunchTransfer([FromBody] BankLaunchDto bankLaunchDto)
    {
        var response = await bankLaunchService.CreateTransferLaunchAsync(bankLaunchDto, 1);

        return Ok(Response<bool>.CreateSuccess(response));
    }

    /// <summary>
    /// Gets all the bank launches of a bank account.
    /// </summary>
    /// <param name="bankAccountId">The bank account ID.</param>
    /// <returns>All the bank launches of the bank account.</returns>
    /// <response code="200">The bank launches were found successfully.</response>
    /// <response code="400">The bank launches could not be found.</response>
    /// <response code="404">The bank account could not be found.</response>
    [HttpGet("get-all-history/{bankAccountId:long}")]
    [Produces("application/json")]
    [ProducesResponseType<Response<IEnumerable<BankLaunchDto>>>(StatusCodes.Status200OK)]
    [ProducesResponseType<Response<string>>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<Response<string>>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAllLaunchByBankAccount(long bankAccountId)
    {
        var response = await bankLaunchService.GetAllLaunchByBankAccountAsync(bankAccountId);

        return Ok(Response<IEnumerable<BankLaunchDto>>.CreateSuccess(response));
    }
}

