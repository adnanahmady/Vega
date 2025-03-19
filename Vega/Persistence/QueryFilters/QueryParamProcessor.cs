using System.Text.RegularExpressions;

using Humanizer;

using Vega.Core.QueryFilters;

namespace Vega.Persistence.QueryFilters;

public abstract class QueryParamProcessor<T> : IQueryParamProcessor<T>
{
    protected IHttpContextAccessor ContextAccessor;
    protected IQueryable<T> Queryable;

    public QueryParamProcessor(IHttpContextAccessor contextAccessor) =>
        ContextAccessor = contextAccessor;

    public IQueryable<T> Apply(IQueryable<T> queryable)
    {
        Queryable = queryable ?? throw new NullReferenceException(
            nameof(queryable));
        var queries = GetQueries();

        foreach (var (k, v) in queries)
        {
            if (!v.Any() || string.IsNullOrWhiteSpace(v)) continue;

            var methodName = k.Pascalize();
            var type = GetType().GetMethod(methodName);

            if (type == null) continue;

            var method = GetType().GetMethod(methodName, new Type[] { typeof(string) });
            method!.Invoke(this, new object[] { v.ToString() });
        }

        return Queryable;
    }

    private IQueryCollection GetQueries() =>
        ContextAccessor.HttpContext!.Request.Query;

    protected object? GetQueryParam(string key) =>
        ContextAccessor.HttpContext!.Request.Query[key];

    protected static bool IsNotNumeric(object value) =>
        !Regex.IsMatch(value.ToString(), @"^\d+$");
}
