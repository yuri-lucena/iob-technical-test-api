using System;
using Iob.Bank.Domain.Data.Dtos;

namespace Iob.Bank.Domain.Interfaces.Services;

public interface IBankLaunchService
{
    Task<IEnumerable<BankLaunchDto>> GetAllLaunchByBankAccountAsync(long bankAccountId);
    Task<bool> CreateCreditLaunchAsync(BankLaunchDto bankLaunchDto, long userId);
    Task<bool> CreateDebitLaunchAsync(BankLaunchDto bankLaunchDto, long userId);
    Task<bool> CreateTransferLaunchAsync(BankLaunchDto bankLaunchDto, long userId);
}
