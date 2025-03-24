using Microsoft.EntityFrameworkCore;

using Vega.Core.Domain;
using Vega.Core.Repositories;

namespace Vega.Persistence.Repositories;

public class PhotoRepository : Repository<VehiclePhoto>, IPhotoRepository
{
    private VegaDbContext VegaDbContext => (Context as VegaDbContext)!;

    public PhotoRepository(VegaDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<VehiclePhoto>> GetPhotosAsync(int vehicleId) =>
        await VegaDbContext.VehiclePhotos
            .Where(p => p.VehicleId == vehicleId)
            .ToListAsync();
}
