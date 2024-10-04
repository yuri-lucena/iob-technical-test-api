using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace Iob.Bank.Domain.Interfaces.Infra.Repositories.Base;

public interface IBaseRepository<TEntity> where TEntity : class
{
    DbSet<TEntity> DbSet { get; set; }
    Task<TEntity> AddAsync(TEntity entity);
    Task<IEnumerable<TEntity>> AddRangeAsync(IEnumerable<TEntity> entities, bool useTransaction = true);
    TEntity Update(TEntity entity);
    void Delete(TEntity entity, bool useTransaction = true);
    void DeleteRange(IEnumerable<TEntity> entities, bool useTransaction = true);
    Task<bool> AnyAsync(Expression<Func<TEntity, bool>> where);
    Task<TEntity?> GetByIdAsync(long id);
    Task<TEntity?> GetByAsync(Expression<Func<TEntity, bool>> where, bool asNoTracking = false);
    Task<IEnumerable<TEntity>> GetListByAsync(Func<TEntity, bool> where, bool asNoTracking = false);
    void Dispose();
}
