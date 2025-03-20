using Microsoft.EntityFrameworkCore;

using Vega.Core.Domain;
using Vega.Core.Repositories;
using Vega.Dtos;
using Vega.ExtensionMethods;

namespace Vega.Persistence.Repositories;

public class VehicleRepository : Repository<Vehicle>, IVehicleRepository
{
    private VegaDbContext VegaDbContext => (Context as VegaDbContext)!;

    public VehicleRepository(
        VegaDbContext context) : base(context)
    {
    }

    public Vehicle? FindWithModelAndFeatures(int id) =>
        VegaDbContext.Vehicles
            .Include(v => v.Model)
            .ThenInclude(m => m.Make)
            .Include(v => v.VehicleFeatures)
            .SingleOrDefault(v => v.Id == id);

    public override IEnumerable<Vehicle> GetAll()
    {
        var queryable = VegaDbContext.Vehicles
            .Include(v => v.Model)
            .ThenInclude(m => m.Make)
            .Include(v => v.VehicleFeatures)
            .AsQueryable();

        if (null != Processor)
        {
            queryable = queryable.QueryParamProcessor(Processor).AsQueryable();
        }

        return queryable.ToList();
    }

    public override PaginationDto<Vehicle> GetAll(int skip, int take)
    {
        var queryable = VegaDbContext.Vehicles
            .OrderBy(v => v.Id)
            .Include(v => v.Model)
            .ThenInclude(m => m.Make)
            .Include(v => v.VehicleFeatures)
            .AsQueryable();

        if (null != Processor)
        {
            queryable = queryable.QueryParamProcessor(Processor).AsQueryable();
        }

        return new PaginationDto<Vehicle>()
        {
            Count = queryable.Count(),
            Items = queryable.Skip(skip * take).Take(take).ToList()
        };
    }

    public async Task<Vehicle?> FindWithModelAndFeaturesAsync(int id) =>
        await VegaDbContext.Vehicles
            .Include(v => v.Model)
            .ThenInclude(m => m.Make)
            .Include(v => v.VehicleFeatures)
            .SingleOrDefaultAsync(v => v.Id == id);

    public IEnumerable<Vehicle> GetPaginatedWithModelAndFeatures(int pageIndex, int pageSize) =>
        VegaDbContext.Vehicles
            .Include(v => v.Model)
            .ThenInclude(m => m.Make)
            .Include(v => v.VehicleFeatures)
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToList();

    public async Task<IEnumerable<Vehicle>> GetPaginatedWithModelAndFeaturesAsync(int pageIndex, int pageSize) =>
        await VegaDbContext.Vehicles
            .Include(v => v.Model)
            .ThenInclude(m => m.Make)
            .Include(v => v.VehicleFeatures)
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

    public async Task<int> ExecuteDeleteAsync(int id) =>
        await VegaDbContext.Vehicles
            .Where(v => v.Id == id)
            .ExecuteDeleteAsync();
}
