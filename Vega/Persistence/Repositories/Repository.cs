using System.Data.Entity;
using System.Linq.Expressions;

using Microsoft.EntityFrameworkCore;

using Vega.Core.Repositories;

using DbContext = Microsoft.EntityFrameworkCore.DbContext;

namespace Vega.Persistence.Repositories;

public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
{
    protected readonly DbContext Context;

    public Repository(DbContext context) => Context = context;

    public TEntity? Get(int id) => Context.Set<TEntity>().Find(id);
    public async Task<TEntity?> GetAsync(int id) => await Context.Set<TEntity>().FindAsync(id);

    public IEnumerable<TEntity> GetAll() => Context.Set<TEntity>().ToList();
    public async Task<IEnumerable<TEntity>> GetAllAsync() =>
        await Context.Set<TEntity>().ToListAsync();

    public IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate) =>
        Context.Set<TEntity>().Where(predicate).ToList();
    public async Task<IEnumerable<TEntity>> FindAsync(
        Expression<Func<TEntity, bool>> predicate) =>
        await Context.Set<TEntity>().Where(predicate).ToListAsync();

    public void Add(TEntity entity) => Context.Set<TEntity>().Add(entity);
    public async Task AddAsync(TEntity entity) =>
        await Context.Set<TEntity>().AddAsync(entity);

    public void AddRange(IEnumerable<TEntity> entities) =>
        Context.Set<TEntity>().AddRange(entities);
    public async Task AddRangeAsync(IEnumerable<TEntity> entities) =>
        await Context.Set<TEntity>().AddRangeAsync(entities);

    public void Remove(TEntity entity) => Context.Set<TEntity>().Remove(entity);

    public void RemoveRange(IEnumerable<TEntity> entities) =>
        Context.Set<TEntity>().RemoveRange(entities);
}
