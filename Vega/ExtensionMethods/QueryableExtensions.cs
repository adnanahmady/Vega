using Vega.Core.QueryFilters;

namespace Vega.ExtensionMethods;

public static class QueryableExtensions
{
    public static IQueryable<T> QueryParamProcessor<T>(this IQueryable<T> queryable, IQueryParamProcessor<T> paramProcessor)
    {
        if (paramProcessor == null)
        {
            throw new ArgumentNullException(nameof(paramProcessor), message: "filter is required");
        }

        return paramProcessor.Apply(queryable);
    }
}
