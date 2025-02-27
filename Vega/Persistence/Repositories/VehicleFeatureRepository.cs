using Vega.Core.Domain;
using Vega.Core.Repositories;

namespace Vega.Persistence.Repositories;

public class VehicleFeatureRepository : Repository<VehicleFeature>, IVehicleFeatureRepository
{
    public VehicleFeatureRepository(VegaDbContext context) : base(context)
    {
    }
}
