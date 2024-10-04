using System.Linq.Expressions;
using Iob.Bank.Domain.Data.Entities.Base;
using Iob.Bank.Domain.Interfaces.Infra.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace Iob.Bank.Infra.Persistence.Repositories.Base;

public class BaseRepository<TEntity> : IBaseRepository<TEntity>
    where TEntity : BaseEntity
{
    private readonly DbContext _dbContext;
    public DbSet<TEntity> DbSet { get; set; }

    public BaseRepository(DataContext dbContext)
    {
        _dbContext = dbContext;
        DbSet = _dbContext.Set<TEntity>();
    }

    public async Task<TEntity> AddAsync(TEntity entity)
    {
        await _dbContext.AddAsync(entity);
        await _dbContext.SaveChangesAsync();

        return entity;
    }

    public async Task<IEnumerable<TEntity>> AddRangeAsync(IEnumerable<TEntity> entities, bool useTransaction = true)
    {
        if (!useTransaction)
        {
            await _dbContext.BulkInsertAsync(entities);
            return entities;
        }

        using var transaction = await _dbContext.Database.BeginTransactionAsync();
        try
        {
            var baseEntities = entities as TEntity[] ?? entities.ToArray();
            await _dbContext.BulkInsertAsync(baseEntities);

            await transaction.CommitAsync();
            return baseEntities;
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            throw new Exception(ex.InnerException?.Message ?? ex.Message);
        }
    }

    public TEntity Update(TEntity entity)
    {
        _dbContext.Entry(entity).State = EntityState.Modified;
        _dbContext.SaveChanges();

        return entity;
    }

    public void Delete(TEntity entity, bool useTransaction = true)
    {
        if (!useTransaction)
        {
            _dbContext.Set<TEntity>().Remove(entity);
            _dbContext.SaveChanges();

            return;
        }

        using var transaction = _dbContext.Database.BeginTransaction();
        try
        {
            _dbContext.Set<TEntity>().Remove(entity);
            _dbContext.SaveChanges();

            transaction.Commit();
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            throw new Exception(ex.InnerException?.Message ?? ex.Message);
        }
    }

    public void DeleteRange(IEnumerable<TEntity> entities, bool useTransaction = true)
    {
        if (!useTransaction)
        {
            _dbContext.Set<TEntity>().BulkDelete(entities);
            return;
        }

        using var transaction = _dbContext.Database.BeginTransaction();
        try
        {
            _dbContext.Set<TEntity>().BulkDelete(entities);

            transaction.Commit();
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            throw new Exception(ex.InnerException?.Message ?? ex.Message);
        }
    }

    public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> where)
    {
        return await _dbContext.Set<TEntity>().AsNoTracking().AnyAsync(where);
    }

    public async Task<TEntity?> GetByIdAsync(long id) =>
         await _dbContext.Set<TEntity>().FindAsync(id);


    public async Task<TEntity?> GetByAsync(Expression<Func<TEntity, bool>> where, bool asNoTracking = false)
    {
        var query = _dbContext.Set<TEntity>().AsQueryable();

        if (asNoTracking)
            query = query.AsNoTracking();

        return await query.FirstOrDefaultAsync(where);
    }

    public async Task<IEnumerable<TEntity>> GetListByAsync(Func<TEntity, bool> where, bool asNoTracking = false)
    {
        var query = _dbContext.Set<TEntity>().AsQueryable();

        if (asNoTracking)
            query = query.AsNoTracking();

        return await Task.FromResult(query.Where(where));
    }

    public virtual void Dispose() => _dbContext.Dispose();
}
