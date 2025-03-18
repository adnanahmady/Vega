using Vega.Core.QueryFilters;

namespace Vega.ExtensionMethods;

public static class QueryableExtensions
{
    public static IQueryable<T> Filter<T>(this IQueryable<T> queryable, IQueryFilter<T> filter)
    {
        if (filter == null)
        {
            throw new ArgumentNullException(nameof(filter), message: "filter is required");
        }

        return filter.Apply(queryable);
    }
}
