using AutoMapper;
using FluentValidation;
using Iob.Bank.Domain.Data.Dtos;
using Iob.Bank.Domain.Data.Entities;
using Iob.Bank.Domain.Data.Enum;
using Iob.Bank.Domain.Exceptions;
using Iob.Bank.Domain.Interfaces.Infra;
using Iob.Bank.Domain.Interfaces.Services;

namespace Iob.Bank.Domain.Services;

public class BankLaunchService(IDbModule dbModule, IValidator<BankLaunchDto> validator, IMapper mapper) : IBankLaunchService
{
    public async Task<bool> CreateCreditLaunchAsync(BankLaunchDto bankLaunchDto, long userId)
    {
        bankLaunchDto.OperationTypeId = (long)OperationTypeEnum.Credit;
        var result = await validator.ValidateAsync(bankLaunchDto);
        if (!result.IsValid)
            throw new ValidationException(result.Errors.First().ErrorMessage);

        using (var transaction = await dbModule.NewTransactionAsync())
        {
            try
            {
                var bankLaunch = mapper.Map<BankLaunch>(bankLaunchDto);
                bankLaunch.CreatedBy = userId;

                await dbModule.BankLaunchRepository.AddAsync(bankLaunch);

                var bankAccount = await dbModule.BankAccountRepository.GetByIdAsync(bankLaunch.DestinationAccountId!.Value);
                bankAccount!.Balance += bankLaunch.Value;

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

    public async Task<bool> CreateDebitLaunchAsync(BankLaunchDto bankLaunchDto, long userId)
    {
        bankLaunchDto.OperationTypeId = (long)OperationTypeEnum.Debit;
        var result = await validator.ValidateAsync(bankLaunchDto);
        if (!result.IsValid)
            throw new ValidationException(result.Errors.First().ErrorMessage);

        using (var transaction = await dbModule.NewTransactionAsync())
        {
            try
            {
                var bankLaunch = mapper.Map<BankLaunch>(bankLaunchDto);
                bankLaunch.CreatedBy = userId;
                await dbModule.BankLaunchRepository.AddAsync(bankLaunch);

                var bankAccount = await dbModule.BankAccountRepository.GetByIdAsync(bankLaunch.DestinationAccountId!.Value);

                bankAccount!.Balance -= bankLaunch.Value;
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

    public async Task<bool> CreateTransferLaunchAsync(BankLaunchDto bankLaunchDto, long userId)
    {
        bankLaunchDto.OperationTypeId = (long)OperationTypeEnum.Transfer;
        var result = await validator.ValidateAsync(bankLaunchDto);
        if (!result.IsValid)
            throw new ValidationException(result.Errors.First().ErrorMessage);

        using (var transaction = await dbModule.NewTransactionAsync())
        {
            try
            {
                var bankAccountOrigin = await dbModule.BankAccountRepository.GetByIdAsync(bankLaunchDto.OriginAccountId!.Value);
                var bankAccountDestination = await dbModule.BankAccountRepository.GetByIdAsync(bankLaunchDto.DestinationAccountId!.Value);

                var bankLaunch = mapper.Map<BankLaunch>(bankLaunchDto);

                bankLaunch.OriginAccountId = bankAccountOrigin!.Id;
                bankLaunch.DestinationAccountId = bankAccountDestination!.Id;
                bankLaunch.CreatedBy = userId;

                await dbModule.BankLaunchRepository.AddAsync(bankLaunch);

                bankAccountOrigin.Balance -= bankLaunch.Value;
                bankAccountDestination.Balance += bankLaunch.Value;

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

    public async Task<IEnumerable<BankLaunchDto>> GetAllLaunchByBankAccountAsync(long bankAccountId)
    {
        var bankAccountExist = await dbModule.BankAccountRepository.AnyAsync(a => a.Id == bankAccountId);
        if (!bankAccountExist)
            throw new BankAccountNotFoundException(bankAccountId);

        var result = (await dbModule.BankLaunchRepository
            .GetListByAsync(w => w.OriginAccountId == bankAccountId || w.DestinationAccountId == bankAccountId))
            .OrderByDescending(m => m.Created)
            .Select(mapper.Map<BankLaunchDto>);

        return result;
    }
}