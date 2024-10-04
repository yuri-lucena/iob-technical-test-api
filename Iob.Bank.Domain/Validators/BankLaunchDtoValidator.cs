using System;
using FluentValidation;
using Iob.Bank.Domain.Data.Dtos;
using Iob.Bank.Domain.Data.Entities;
using Iob.Bank.Domain.Data.Enum;
using Iob.Bank.Domain.Interfaces.Infra;

namespace Iob.Bank.Domain.Validators;

public class BankLaunchDtoValidator : AbstractValidator<BankLaunchDto>
{
    private BankAccount? _bankAccountDestination;
    private BankAccount? _bankAccountOrigin;

    public BankLaunchDtoValidator(IDbModule dbModule)
    {
        RuleFor(bankLaunch => bankLaunch)
            .Custom((bankLaunch, context) =>
            {
                _bankAccountDestination = dbModule.BankAccountRepository.GetByIdAsync(bankLaunch.DestinationAccountId ?? 0).GetAwaiter().GetResult();
                _bankAccountOrigin = dbModule.BankAccountRepository.GetByIdAsync(bankLaunch.OriginAccountId ?? 0).GetAwaiter().GetResult();

                if ((bankLaunch.OperationTypeId == (long)OperationTypeEnum.Credit ||
                        bankLaunch.OperationTypeId == (long)OperationTypeEnum.Debit) &&
                        _bankAccountDestination == null)
                    context.AddFailure(nameof(bankLaunch.DestinationAccountId), "Id da conta de destino deve ser válido");

                if (bankLaunch.OperationTypeId == (long)OperationTypeEnum.Debit &&
                        (_bankAccountDestination?.Balance ?? 0) < bankLaunch.Value)
                    context.AddFailure(nameof(bankLaunch.Value), "Valor deve ser menor ou igual ao seu saldo");

                if (bankLaunch.OperationTypeId == (long)OperationTypeEnum.Transfer)
                {
                    if (_bankAccountDestination == null)
                        context.AddFailure(nameof(bankLaunch.DestinationAccountId), "Id da conta de destino deve ser válido");

                    if (_bankAccountOrigin == null)
                        context.AddFailure(nameof(bankLaunch.OriginAccountId), "Id da conta de origem deve ser válido");
                    else
                    {
                        if (bankLaunch.Value > _bankAccountOrigin.Balance)
                            context.AddFailure(nameof(bankLaunch.Value), "Valor deve ser menor ou igual ao seu saldo");
                    }
                }
            });

        RuleFor(r => r.Value)
            .GreaterThan(0)
            .WithMessage("Valor deve ser maior que zero");

        RuleFor(r => r.DestinationAccountId)
            .GreaterThan(0)
            .WithMessage("Id da conta de destino deve ser maior que zero");

        RuleFor(r => r.OperationTypeId)
            .Must(operationType => Enum.IsDefined(typeof(OperationTypeEnum), operationType))
            .WithMessage("Id do tipo de operação deve ser válido");

    }
}

