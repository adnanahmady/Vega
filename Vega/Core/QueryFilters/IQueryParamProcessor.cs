namespace Vega.Core.QueryFilters;

public interface IQueryParamProcessor<T>
{
    IQueryable<T> Apply(IQueryable<T> queryable);
}
