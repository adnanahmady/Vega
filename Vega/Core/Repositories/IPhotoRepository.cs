using Vega.Core.Domain;

namespace Vega.Core.Repositories;

public interface IPhotoRepository : IRepository<VehiclePhoto>
{
    Task<IEnumerable<VehiclePhoto>> GetPhotosAsync(int vehicleId);
}
