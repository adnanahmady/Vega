using System.Text.Json;

using Vega.Core.Domain;
using Vega.Core.QueryFilters;

namespace Vega.Persistence.QueryFilters;

public class VehiclesListFilter : QueryFilter<Vehicle>, IVehiclesListFilter
{
    public VehiclesListFilter(IHttpContextAccessor contextAccessor) : base(contextAccessor)
    {
    }

    // the method handles the given sortBy query string
    public void SortBy(object column)
    {
        var c = GetColumnMap((string)column);

        if (GetQueryParam("sortDirection")?.ToString() == "desc")
        {
            Queryable = Queryable.OrderByDescending(c).AsQueryable();

            return;
        }

        Queryable = Queryable.OrderBy(c).AsQueryable();
    }

    private Func<Vehicle, object> GetColumnMap(string column) => column.ToLower() switch
        {
            "make" => v => v.Model.Make.Name,
            "model" => v => v.Model.Name,
            "contactName" => v => v.ContactName,
            _ => v => v.Id,
        };
}
