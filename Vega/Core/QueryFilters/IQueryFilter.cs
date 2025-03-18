namespace Vega.Core.QueryFilters;

public interface IQueryFilter<T>
{
    IQueryable<T> Apply(IQueryable<T> queryable);
}
