using AutoMapper;
using Iob.Bank.Domain.Data.Dtos;
using Iob.Bank.Domain.Data.Entities;
using Iob.Bank.Domain.Exceptions;
using Iob.Bank.Domain.Interfaces.Infra;
using Iob.Bank.Domain.Interfaces.Services;

namespace Iob.Bank.Domain.Services;

public class BankAccountService(IMapper mapper, IDbModule dbModule) : IBankAccountService
{
    public async Task<BankAccountDto> CreateAsync(BankAccountDto bankAccountDto, long userId)
    {
        var bankAccount = mapper.Map<BankAccount>(bankAccountDto);

        using (var transaction = await dbModule.NewTransactionAsync())
        {
            try
            {
                bankAccount.UserId = bankAccountDto.UserId;
                bankAccount = await dbModule.BankAccountRepository.AddAsync(bankAccount);

                await dbModule.CommitChangesAsync(transaction);

                var ret = mapper.Map<BankAccountDto>(bankAccount);
                return ret;
            }
            catch (Exception ex)
            {
                await dbModule.RollbackTransactionAsync(transaction);
                throw new CreateBankAccountException(ex.InnerException?.Message ?? ex.Message);
            }
        }
    }

    public async Task<BankAccountDto> UpdateAsync(BankAccountDto bankAccountDto, long userId)
    {
        using (var transaction = await dbModule.NewTransactionAsync())
        {
            try
            {
                var bankAccount = await dbModule.BankAccountRepository.GetByIdAsync(bankAccountDto.Id) ??
                    throw new BankAccountNotFoundException(bankAccountDto.Id);
                bankAccount = dbModule.BankAccountRepository.Update(bankAccount);
                await dbModule.CommitChangesAsync(transaction);
                return mapper.Map<BankAccountDto>(bankAccount);
            }
            catch (Exception)
            {
                await dbModule.RollbackTransactionAsync(transaction);
                throw;
            }
        }
    }

    public async Task<bool> DeleteAsync(long id, long userId)
    {
        using (var transaction = await dbModule.NewTransactionAsync())
        {
            try
            {
                var bankAccount = await dbModule.BankAccountRepository.GetByIdAsync(id) ??
                    throw new BankAccountNotFoundException(id);

                bankAccount.Active = false;
                await dbModule.CommitChangesAsync(transaction);
                return true;
            }
            catch (Exception)
            {
                await dbModule.RollbackTransactionAsync(transaction);
                throw;
            }
        }
    }

    public async Task<BankAccountDto> GetAsync(long id, long userId)
    {
        return mapper.Map<BankAccountDto>((await dbModule.BankAccountRepository.GetByAsync(w => w.Id == id && w.UserId == userId)) ??
            throw new BankAccountNotFoundException(id));
    }

    public async Task<IEnumerable<BankAccountDto>> GetAllAsync(long userId)
    {
        return (await dbModule.BankAccountRepository.GetListByAsync(w => w.Active && w.UserId == userId, asNoTracking: true))
            .Select(mapper.Map<BankAccountDto>);
    }

    public async Task<string> GetAccountBalanceAsync(long bankAccountId, long userId)
    {

        var bankAccount = await dbModule.BankAccountRepository.GetByAsync(w => w.Id == bankAccountId &&
            w.Active && w.UserId == userId, asNoTracking: true) ?? throw new BankAccountNotFoundException(bankAccountId);

        return $"R$ {bankAccount.Balance:F2}";
    }
}
