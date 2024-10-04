using System;
using FluentValidation.TestHelper;
using Iob.Bank.Domain.Data.Dtos;
using Iob.Bank.Domain.Data.Entities;
using Iob.Bank.Domain.Data.Enum;
using Iob.Bank.Domain.Interfaces.Infra;
using Iob.Bank.Domain.Validators;
using Moq;

namespace Iob.Bank.Tests.Validators;

public class BankLaunchDtoValidatorTests
{
    private readonly BankLaunchDtoValidator _validator;
    private readonly Mock<IDbModule> _dbModuleMock;
    public BankLaunchDtoValidatorTests()
    {
        _dbModuleMock = new Mock<IDbModule>();
        _validator = new BankLaunchDtoValidator(_dbModuleMock.Object);
        CreateMock();
    }

    private void CreateMock()
    {
        _dbModuleMock.Setup(db => db.BankAccountRepository.GetByIdAsync(1)).ReturnsAsync(new BankAccount { Balance = 100 });
    }

    [Fact]
    public async Task ValidBankLaunchDto_ReturnsNoErrors()
    {
        var bankLaunchDto = new BankLaunchDto
        {
            DestinationAccountId = 1,
            OriginAccountId = 1,
            OperationTypeId = (long)OperationTypeEnum.Transfer,
            Value = 10
        };

        var result = await _validator.TestValidateAsync(bankLaunchDto);

        result.ShouldNotHaveValidationErrorFor(b => b.DestinationAccountId);
        result.ShouldNotHaveValidationErrorFor(b => b.OriginAccountId);
        result.ShouldNotHaveValidationErrorFor(b => b.OperationTypeId);
        result.ShouldNotHaveValidationErrorFor(b => b.Value);
    }

    [Fact]
    public async Task InvalidDestinationAccountIdForCreditDebitOperation_ReturnsError()
    {

        var bankLaunchDto = new BankLaunchDto
        {
            DestinationAccountId = 0,
            OperationTypeId = (long)OperationTypeEnum.Credit
        };

        var result = await _validator.TestValidateAsync(bankLaunchDto);

        result.ShouldHaveValidationErrorFor(b => b.DestinationAccountId);
    }

    [Fact]
    public async Task InvalidDestinationAccountIdForTransferOperation_ReturnsError()
    {

        var bankLaunchDto = new BankLaunchDto
        {
            DestinationAccountId = 0,
            OperationTypeId = (long)OperationTypeEnum.Transfer
        };

        var result = await _validator.TestValidateAsync(bankLaunchDto);

        result.ShouldHaveValidationErrorFor(b => b.DestinationAccountId);
    }

    [Fact]
    public async Task InvalidOriginAccountIdForTransferOperation_ReturnsError()
    {

        var bankLaunchDto = new BankLaunchDto
        {
            OriginAccountId = 0,
            OperationTypeId = (long)OperationTypeEnum.Transfer
        };

        var result = await _validator.TestValidateAsync(bankLaunchDto);

        result.ShouldHaveValidationErrorFor(b => b.OriginAccountId);
    }

    [Fact]
    public async Task InsufficientBalanceForTransferOperation_ReturnsError()
    {

        var bankLaunchDto = new BankLaunchDto
        {
            OriginAccountId = 1,
            OperationTypeId = (long)OperationTypeEnum.Transfer,
            Value = 1000
        };
        _dbModuleMock.Setup(db => db.BankAccountRepository.GetByIdAsync(1)).ReturnsAsync(new BankAccount() { Balance = 100 });

        var result = await _validator.TestValidateAsync(bankLaunchDto);

        result.ShouldHaveValidationErrorFor(b => b.Value);
    }

    [Fact]
    public async Task DebitOperationWithInsufficientBalance_ReturnsError()
    {
        var bankLaunchDto = new BankLaunchDto
        {
            DestinationAccountId = 1,
            Value = 100,
            OperationTypeId = (long)OperationTypeEnum.Debit
        };
        _dbModuleMock.Setup(m => m.BankAccountRepository.GetByIdAsync(It.IsAny<long>()))
            .ReturnsAsync(new BankAccount { Balance = 50 });
        var result = await _validator.TestValidateAsync(bankLaunchDto);

        result.ShouldHaveValidationErrorFor(b => b.Value);
    }

    [Fact]
    public async Task InvalidOperationTypeId_ReturnsError()
    {

        var bankLaunchDto = new BankLaunchDto
        {
            OperationTypeId = 100
        };

        var result = await _validator.TestValidateAsync(bankLaunchDto);

        result.ShouldHaveValidationErrorFor(b => b.OperationTypeId);
    }

    [Fact]
    public async Task InvalidValue_ReturnsError()
    {

        var bankLaunchDto = new BankLaunchDto
        {
            Value = -1
        };

        var result = await _validator.TestValidateAsync(bankLaunchDto);

        result.ShouldHaveValidationErrorFor(b => b.Value);
    }

    [Fact]
    public async Task InvalidDestinationAccountId_ReturnsError()
    {

        var bankLaunchDto = new BankLaunchDto
        {
            DestinationAccountId = -1
        };

        var result = await _validator.TestValidateAsync(bankLaunchDto);

        result.ShouldHaveValidationErrorFor(b => b.DestinationAccountId);
    }
}
