using Iob.Bank.Domain.Data.Entities;
using Iob.Bank.Domain.Interfaces.Infra.Repositories;
using Iob.Bank.Domain.Interfaces.Infra.Repositories.Base;
using Microsoft.EntityFrameworkCore.Storage;

namespace Iob.Bank.Domain.Interfaces.Infra;

public interface IDbModule
{
    public IBaseRepository<BankAccount> BankAccountRepository { get; }
    public IBaseRepository<BankLaunch> BankLaunchRepository { get; }
    public IBaseRepository<OperationType> OperationTypeRepository { get; }
    public IUserRepository UserRepository { get; }
    public IBaseRepository<UserType> UserTypeRepository { get; }
    Task<IDbContextTransaction> NewTransactionAsync();
    Task RollbackTransactionAsync(IDbContextTransaction transaction);
    Task CommitChangesAsync(IDbContextTransaction transaction);
    Task SaveChangesAsync();
}
