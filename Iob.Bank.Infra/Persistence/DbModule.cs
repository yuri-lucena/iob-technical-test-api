using AutoMapper;
using Iob.Bank.Domain.Data.Entities;
using Iob.Bank.Domain.Interfaces.Infra;
using Iob.Bank.Domain.Interfaces.Infra.Repositories;
using Iob.Bank.Domain.Interfaces.Infra.Repositories.Base;
using Iob.Bank.Infra.Persistence.Repositories;
using Iob.Bank.Infra.Persistence.Repositories.Base;
using Microsoft.EntityFrameworkCore.Storage;

namespace Iob.Bank.Infra.Persistence;

public class DbModule(DataContext dataContext, IMapper mapper) : IDbModule
{
    private IBaseRepository<BankAccount>? bankAccountRepository;
    public IBaseRepository<BankAccount> BankAccountRepository => bankAccountRepository ??= new BaseRepository<BankAccount>(dataContext);

    private IBaseRepository<BankLaunch>? bankLaunchRepository;
    public IBaseRepository<BankLaunch> BankLaunchRepository => bankLaunchRepository ??= new BaseRepository<BankLaunch>(dataContext);

    private IBaseRepository<OperationType>? operationTypeRepository;
    public IBaseRepository<OperationType> OperationTypeRepository => operationTypeRepository ??= new BaseRepository<OperationType>(dataContext);

    private IUserRepository? userRepository;
    public IUserRepository UserRepository => userRepository ??= new UserRepository(mapper, dataContext);

    private IBaseRepository<UserType>? userTypeRepository;
    public IBaseRepository<UserType> UserTypeRepository => userTypeRepository ??= new BaseRepository<UserType>(dataContext);

    public async Task<IDbContextTransaction> NewTransactionAsync() => await dataContext.Database.BeginTransactionAsync();
    public async Task RollbackTransactionAsync(IDbContextTransaction transaction) => await transaction.RollbackAsync();
    public async Task SaveChangesAsync() => await dataContext.SaveChangesAsync();
    public async Task CommitChangesAsync(IDbContextTransaction transaction)
    {
        await dataContext.SaveChangesAsync();

        if (transaction != null)
        {
            await transaction.CommitAsync();
        }
    }
}
