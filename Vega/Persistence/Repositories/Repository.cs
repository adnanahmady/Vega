using System.Data.Entity;
using System.Linq.Expressions;

using Microsoft.EntityFrameworkCore;

using Vega.Core.QueryFilters;
using Vega.Core.Repositories;
using Vega.Dtos;

using DbContext = Microsoft.EntityFrameworkCore.DbContext;

namespace Vega.Persistence.Repositories;

public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
{
    protected readonly DbContext Context;
    protected IQueryParamProcessor<TEntity>? Processor;

    public IRepository<TEntity> QueryParamProcessor(IQueryParamProcessor<TEntity> processor)
    {
        Processor = processor;

        return this;
    }

    public Repository(DbContext context) => Context = context;

    public virtual TEntity? Get(int id) => Context.Set<TEntity>().Find(id);
    public virtual async Task<TEntity?> GetAsync(int id) => await Context.Set<TEntity>().FindAsync(id);

    public virtual int CountAll() => Context.Set<TEntity>().Count();
    public virtual IEnumerable<TEntity> GetAll() => Context.Set<TEntity>().ToList();

    public virtual PaginationDto<TEntity> GetAll(int skip, int take) => new()
    {
        Count = Context.Set<TEntity>().Count(),
        Items = Context.Set<TEntity>().Skip(skip).Take(take).ToList(),
    };

    public virtual async Task<IEnumerable<TEntity>> GetAllAsync() =>
        await Context.Set<TEntity>().ToListAsync();

    public virtual IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate) =>
        Context.Set<TEntity>().Where(predicate).ToList();
    public virtual async Task<IEnumerable<TEntity>> FindAsync(
        Expression<Func<TEntity, bool>> predicate) =>
        await Context.Set<TEntity>().Where(predicate).ToListAsync();

    public virtual void Add(TEntity entity) => Context.Set<TEntity>().Add(entity);
    public virtual async Task AddAsync(TEntity entity) =>
        await Context.Set<TEntity>().AddAsync(entity);

    public virtual void AddRange(IEnumerable<TEntity> entities) =>
        Context.Set<TEntity>().AddRange(entities);
    public virtual async Task AddRangeAsync(IEnumerable<TEntity> entities) =>
        await Context.Set<TEntity>().AddRangeAsync(entities);

    public virtual void Remove(TEntity entity) => Context.Set<TEntity>().Remove(entity);

    public virtual void RemoveRange(IEnumerable<TEntity> entities) =>
        Context.Set<TEntity>().RemoveRange(entities);
}
