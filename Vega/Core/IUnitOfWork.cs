using Vega.Core.Repositories;

namespace Vega.Core;

public interface IUnitOfWork : IDisposable
{
    IVehicleRepository Vehicles { get; }
    IMakeRepository Makes { get; }
    IModelRepository Models { get; }
    IVehicleFeatureRepository VehicleFeatures { get; }
    IPhotoRepository VehiclePhotos { get; }

    int Complete();
    Task<int> CompleteAsync();
}
