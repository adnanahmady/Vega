using System.Linq.Expressions;

namespace Vega.Core.Repositories;

public interface IRepository<TEntity> where TEntity : class
{
    TEntity? Get(int id);
    Task<TEntity?> GetAsync(int id);
    int CountAll();
    IEnumerable<TEntity> GetAll();
    IEnumerable<TEntity> GetAll(int skip, int take);
    Task<IEnumerable<TEntity>> GetAllAsync();
    IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate);
    Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate);

    void Add(TEntity entity);
    Task AddAsync(TEntity entity);
    void AddRange(IEnumerable<TEntity> entities);
    Task AddRangeAsync(IEnumerable<TEntity> entities);

    void Remove(TEntity entity);
    void RemoveRange(IEnumerable<TEntity> entities);
}
