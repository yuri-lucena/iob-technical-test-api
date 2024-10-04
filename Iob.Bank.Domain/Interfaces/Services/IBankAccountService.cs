using Iob.Bank.Domain.Data.Dtos;

namespace Iob.Bank.Domain.Interfaces.Services;

public interface IBankAccountService
{
    Task<BankAccountDto> CreateAsync(BankAccountDto bankAccountDto, long userId);
    Task<BankAccountDto> UpdateAsync(BankAccountDto bankAccountDto, long userId);
    Task<bool> DeleteAsync(long id, long userId);
    Task<BankAccountDto> GetAsync(long id, long userId);
    Task<IEnumerable<BankAccountDto>> GetAllAsync(long userId);
    Task<string> GetAccountBalanceAsync(long bankAccountId, long userId);
}
