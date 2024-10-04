using Moq;
using Iob.Bank.Domain.Data.Dtos;
using Iob.Bank.Domain.Data.Entities;
using Iob.Bank.Domain.Exceptions;
using Iob.Bank.Domain.Interfaces.Infra;
using AutoMapper;
using Iob.Bank.Domain.Interfaces.Infra.Repositories.Base;
using Iob.Bank.Domain.Services;
using Microsoft.EntityFrameworkCore.Storage;
using System.Linq.Expressions;

namespace Iob.Bank.Tests.Services;

public class BankAccountServiceTests
{
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IDbModule> _dbModuleMock;
    private readonly Mock<IBaseRepository<BankAccount>> _bankAccountRepositoryMock;
    private readonly BankAccountService _bankAccountService;
    public BankAccountServiceTests()
    {
        _mapperMock = new Mock<IMapper>();
        _dbModuleMock = new Mock<IDbModule>();
        _bankAccountRepositoryMock = new Mock<IBaseRepository<BankAccount>>();
        _dbModuleMock.SetupGet(x => x.BankAccountRepository).Returns(_bankAccountRepositoryMock.Object);
        _bankAccountService = new BankAccountService(_mapperMock.Object, _dbModuleMock.Object);
    }

    [Fact]
    public async Task CreateAsync_SuccessfulCreation_ReturnsBankAccountDto()
    {
        var bankAccountDto = new BankAccountDto();
        var bankAccount = new BankAccount();
        _mapperMock.Setup(x => x.Map<BankAccount>(bankAccountDto)).Returns(bankAccount);
        _mapperMock.Setup(x => x.Map<BankAccountDto>(bankAccount)).Returns(bankAccountDto);
        _bankAccountRepositoryMock.Setup(x => x.AddAsync(bankAccount)).ReturnsAsync(bankAccount);

        var result = await _bankAccountService.CreateAsync(bankAccountDto, 1);

        Assert.NotNull(result);
        Assert.IsType<BankAccountDto>(result);
        _bankAccountRepositoryMock.Verify(x => x.AddAsync(bankAccount), Times.Once);
        _dbModuleMock.Verify(x => x.CommitChangesAsync(It.IsAny<IDbContextTransaction>()), Times.Once);
        _mapperMock.Verify(x => x.Map<BankAccountDto>(bankAccount), Times.Once);
        _mapperMock.Verify(x => x.Map<BankAccount>(bankAccountDto), Times.Once);
        _dbModuleMock.Verify(x => x.RollbackTransactionAsync(It.IsAny<IDbContextTransaction>()), Times.Never);
        _dbModuleMock.Verify(x => x.NewTransactionAsync(), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_AddAsyncFails_ThrowsCreateBankAccountException()
    {
        var bankAccountDto = new BankAccountDto();
        var bankAccount = new BankAccount();
        _mapperMock.Setup(x => x.Map<BankAccount>(bankAccountDto)).Returns(bankAccount);
        _bankAccountRepositoryMock.Setup(x => x.AddAsync(bankAccount)).ThrowsAsync(new Exception());

        await Assert.ThrowsAsync<CreateBankAccountException>(() => _bankAccountService.CreateAsync(bankAccountDto, 1));
    }

    [Fact]
    public async Task CreateAsync_CommitChangesFails_ThrowsCreateBankAccountException()
    {
        var bankAccountDto = new BankAccountDto();
        var bankAccount = new BankAccount();
        _mapperMock.Setup(x => x.Map<BankAccount>(bankAccountDto)).Returns(bankAccount);
        _bankAccountRepositoryMock.Setup(x => x.AddAsync(bankAccount)).ReturnsAsync(bankAccount);
        _dbModuleMock.Setup(x => x.CommitChangesAsync(It.IsAny<IDbContextTransaction>())).ThrowsAsync(new Exception());

        await Assert.ThrowsAsync<CreateBankAccountException>(() => _bankAccountService.CreateAsync(bankAccountDto, 1));
    }

    [Fact]
    public async Task CreateAsync_ExceptionOccurs_RollbackTransactionIsCalled()
    {
        var bankAccountDto = new BankAccountDto();
        var bankAccount = new BankAccount();
        _mapperMock.Setup(x => x.Map<BankAccount>(bankAccountDto)).Returns(bankAccount);
        _bankAccountRepositoryMock.Setup(x => x.AddAsync(bankAccount)).ThrowsAsync(new Exception());
        var transactionMock = new Mock<IDbContextTransaction>();
        _dbModuleMock.Setup(x => x.NewTransactionAsync()).ReturnsAsync(transactionMock.Object);

        await Assert.ThrowsAsync<CreateBankAccountException>(() => _bankAccountService.CreateAsync(bankAccountDto, 1));

        _dbModuleMock.Verify(x => x.RollbackTransactionAsync(transactionMock.Object), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_SuccessfulUpdate_ReturnsBankAccountDto()
    {
        var bankAccountDto = new BankAccountDto { Id = 1 };
        var bankAccount = new BankAccount { Id = 1 };
        _mapperMock.Setup(x => x.Map<BankAccount>(bankAccountDto)).Returns(bankAccount);
        _mapperMock.Setup(x => x.Map<BankAccountDto>(bankAccount)).Returns(bankAccountDto);
        _bankAccountRepositoryMock.Setup(x => x.GetByIdAsync(bankAccountDto.Id)).ReturnsAsync(bankAccount);
        _bankAccountRepositoryMock.Setup(x => x.Update(bankAccount)).Returns(bankAccount);
        _dbModuleMock.Setup(x => x.CommitChangesAsync(It.IsAny<IDbContextTransaction>())).Verifiable();

        var result = await _bankAccountService.UpdateAsync(bankAccountDto, 1);

        Assert.NotNull(result);
        Assert.IsType<BankAccountDto>(result);
        _bankAccountRepositoryMock.Verify(x => x.Update(bankAccount), Times.Once);
        _bankAccountRepositoryMock.Verify(x => x.GetByIdAsync(bankAccountDto.Id), Times.Once);
        _dbModuleMock.Verify(x => x.CommitChangesAsync(It.IsAny<IDbContextTransaction>()), Times.Once);
        _dbModuleMock.Verify(x => x.RollbackTransactionAsync(It.IsAny<IDbContextTransaction>()), Times.Never);
        _dbModuleMock.Verify(x => x.NewTransactionAsync(), Times.Once);
        _mapperMock.Verify(x => x.Map<BankAccountDto>(bankAccount), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_BankAccountNotFound_ThrowsBankAccountNotFoundException()
    {
        var bankAccountDto = new BankAccountDto { Id = 1 };
        _bankAccountRepositoryMock.Setup(x => x.GetByIdAsync(bankAccountDto.Id)).ReturnsAsync((BankAccount?)null);

        await Assert.ThrowsAsync<BankAccountNotFoundException>(() => _bankAccountService.UpdateAsync(bankAccountDto, 1));
        _bankAccountRepositoryMock.Verify(x => x.GetByIdAsync(bankAccountDto.Id), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_UpdateFails_RollbackTransactionIsCalled()
    {
        var bankAccountDto = new BankAccountDto { Id = 1 };
        var bankAccount = new BankAccount { Id = 1 };
        _mapperMock.Setup(x => x.Map<BankAccount>(bankAccountDto)).Returns(bankAccount);
        _bankAccountRepositoryMock.Setup(x => x.GetByIdAsync(bankAccountDto.Id)).ReturnsAsync(bankAccount);
        _bankAccountRepositoryMock.Setup(x => x.Update(bankAccount)).Throws(new Exception());
        var transactionMock = new Mock<IDbContextTransaction>();
        _dbModuleMock.Setup(x => x.NewTransactionAsync()).ReturnsAsync(transactionMock.Object);

        await Assert.ThrowsAsync<Exception>(() => _bankAccountService.UpdateAsync(bankAccountDto, 1));
        _dbModuleMock.Verify(x => x.RollbackTransactionAsync(transactionMock.Object), Times.Once);
        _dbModuleMock.Verify(x => x.NewTransactionAsync(), Times.Once);
    }
    [Fact]
    public async Task UpdateAsync_ExceptionIsThrown_ThrowsException()
    {
        var bankAccountDto = new BankAccountDto { Id = 1 };
        var bankAccount = new BankAccount { Id = 1 };
        _mapperMock.Setup(x => x.Map<BankAccount>(bankAccountDto)).Returns(bankAccount);
        _bankAccountRepositoryMock.Setup(x => x.GetByIdAsync(bankAccountDto.Id)).ReturnsAsync(bankAccount);
        _bankAccountRepositoryMock.Setup(x => x.Update(bankAccount)).Throws(new Exception());

        await Assert.ThrowsAsync<Exception>(() => _bankAccountService.UpdateAsync(bankAccountDto, 1));
        _dbModuleMock.Verify(x => x.RollbackTransactionAsync(It.IsAny<IDbContextTransaction>()), Times.Once);
        _dbModuleMock.Verify(x => x.NewTransactionAsync(), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_SuccessfulDeletion_ReturnsTrue()
    {

        var bankAccount = new BankAccount { Id = 1 };
        _bankAccountRepositoryMock.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(bankAccount);
        _dbModuleMock.Setup(x => x.CommitChangesAsync(It.IsAny<IDbContextTransaction>())).Verifiable();

        var result = await _bankAccountService.DeleteAsync(1, 1);

        Assert.True(result);
        _bankAccountRepositoryMock.Verify(x => x.GetByIdAsync(1), Times.Once);
        _dbModuleMock.Verify(x => x.CommitChangesAsync(It.IsAny<IDbContextTransaction>()), Times.Once);
        _dbModuleMock.Verify(x => x.RollbackTransactionAsync(It.IsAny<IDbContextTransaction>()), Times.Never);
        _dbModuleMock.Verify(x => x.NewTransactionAsync(), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_BankAccountNotFound_ThrowsBankAccountNotFoundException()
    {
        _bankAccountRepositoryMock.Setup(x => x.GetByIdAsync(1)).ReturnsAsync((BankAccount?)null);

        await Assert.ThrowsAsync<BankAccountNotFoundException>(() => _bankAccountService.DeleteAsync(1, 1));
        _bankAccountRepositoryMock.Verify(x => x.GetByIdAsync(1), Times.Once);
        _dbModuleMock.Verify(x => x.RollbackTransactionAsync(It.IsAny<IDbContextTransaction>()), Times.Once);
        _dbModuleMock.Verify(x => x.NewTransactionAsync(), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_ExceptionOccurs_RollbackTransactionIsCalled()
    {
        var bankAccount = new BankAccount { Id = 1 };
        _bankAccountRepositoryMock.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(bankAccount);
        _dbModuleMock.Setup(x => x.CommitChangesAsync(It.IsAny<IDbContextTransaction>())).ThrowsAsync(new Exception());
        var transactionMock = new Mock<IDbContextTransaction>();
        _dbModuleMock.Setup(x => x.NewTransactionAsync()).ReturnsAsync(transactionMock.Object);

        await Assert.ThrowsAsync<Exception>(() => _bankAccountService.DeleteAsync(1, 1));
        _dbModuleMock.Verify(x => x.RollbackTransactionAsync(transactionMock.Object), Times.Once);
        _dbModuleMock.Verify(x => x.NewTransactionAsync(), Times.Once);
        _bankAccountRepositoryMock.Verify(x => x.GetByIdAsync(1), Times.Once);
    }

    [Fact]
    public async Task GetAsync_SuccessfulRetrieval_ReturnsBankAccountDto()
    {
        var bankAccount = new BankAccount { Id = 1, UserId = 1 };
        _mapperMock.Setup(x => x.Map<BankAccountDto>(bankAccount)).Returns(new BankAccountDto());
        _bankAccountRepositoryMock.Setup(x => x.GetByAsync(ba => ba.Id == 1 && ba.UserId == 1, false)).ReturnsAsync(bankAccount);

        var result = await _bankAccountService.GetAsync(1, 1);

        Assert.NotNull(result);
        Assert.IsType<BankAccountDto>(result);
    }

    [Fact]
    public async Task GetAsync_BankAccountNotFound_ThrowsBankAccountNotFoundException()
    {
        _bankAccountRepositoryMock.Setup(x => x.GetByAsync(ba => ba.Id == 1 && ba.UserId == 1, false)).ReturnsAsync((BankAccount?)null);

        await Assert.ThrowsAsync<BankAccountNotFoundException>(() => _bankAccountService.GetAsync(1, 1));
    }

    [Fact]
    public async Task GetAsync_NonMatchingUserId_ReturnsNull()
    {
        var bankAccount = new BankAccount { Id = 1, UserId = 2 };
        _bankAccountRepositoryMock.Setup(x => x.GetByAsync(ba => ba.Id == 1 && ba.UserId == 1, false)).ReturnsAsync(bankAccount);

        var result = await _bankAccountService.GetAsync(1, 1);

        Assert.Null(result);
    }

    [Fact]
    public async Task GetAsync_RepositoryThrowsException_ThrowsException()
    {
        _bankAccountRepositoryMock.Setup(x => x.GetByAsync(ba => ba.Id == 1 && ba.UserId == 1, false)).ThrowsAsync(new Exception());

        await Assert.ThrowsAsync<Exception>(() => _bankAccountService.GetAsync(1, 1));
    }

    [Fact]
    public async Task GetAccountBalanceAsync_ValidBankAccountIdAndUserId_ReturnsBalance()
    {
        var bankAccountId = 1L;
        var userId = 1L;
        var bankAccount = new BankAccount { Id = bankAccountId, Active = true, UserId = userId, Balance = 100.0m };
        _dbModuleMock.Setup(db => db.BankAccountRepository.GetByAsync(It.IsAny<Expression<Func<BankAccount, bool>>>(), true))
            .ReturnsAsync(bankAccount);

        var result = await _bankAccountService.GetAccountBalanceAsync(bankAccountId, userId);

        Assert.Equal("100.0", result);
        _bankAccountRepositoryMock.Verify(x => x.GetByAsync(It.IsAny<Expression<Func<BankAccount, bool>>>(), true), Times.Once);
    }

    [Fact]
    public async Task GetAccountBalanceAsync_InvalidBankAccountId_ThrowsBankAccountNotFoundException()
    {
        var bankAccountId = 1L;
        var userId = 1L;
        _dbModuleMock.Setup(db => db.BankAccountRepository.GetByAsync(It.IsAny<Expression<Func<BankAccount, bool>>>(), true))
            .ReturnsAsync((BankAccount?)null);

        await Assert.ThrowsAsync<BankAccountNotFoundException>(() => _bankAccountService.GetAccountBalanceAsync(bankAccountId, userId));
        _bankAccountRepositoryMock.Verify(x => x.GetByAsync(It.IsAny<Expression<Func<BankAccount, bool>>>(), true), Times.Once);
    }

    [Fact]
    public async Task GetAccountBalanceAsync_InvalidUserId_ThrowsBankAccountNotFoundException()
    {
        var bankAccountId = 1L;
        var userId = 2L;
        var bankAccount = new BankAccount { Id = bankAccountId, Active = true, UserId = 1L, Balance = 100.0m };
        _dbModuleMock.Setup(db => db.BankAccountRepository.GetByAsync(w => w.Id == bankAccountId &&
            w.Active && w.UserId == userId, true))
            .ReturnsAsync((BankAccount?)null);

        await Assert.ThrowsAsync<BankAccountNotFoundException>(() => _bankAccountService.GetAccountBalanceAsync(bankAccountId, userId));
        _bankAccountRepositoryMock.Verify(x => x.GetByAsync(It.IsAny<Expression<Func<BankAccount, bool>>>(), true), Times.Once);
    }

    [Fact]
    public async Task GetAccountBalanceAsync_BankAccountNotFound_ThrowsBankAccountNotFoundException()
    {
        var bankAccountId = 1L;
        var userId = 1L;
        _dbModuleMock.Setup(db => db.BankAccountRepository.GetByAsync(It.IsAny<Expression<Func<BankAccount, bool>>>(), true))
            .ReturnsAsync((BankAccount?)null);

        await Assert.ThrowsAsync<BankAccountNotFoundException>(() => _bankAccountService.GetAccountBalanceAsync(bankAccountId, userId));
        _bankAccountRepositoryMock.Verify(x => x.GetByAsync(It.IsAny<Expression<Func<BankAccount, bool>>>(), true), Times.Once);
    }
}