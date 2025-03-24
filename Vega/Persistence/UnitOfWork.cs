using Vega.Core;
using Vega.Core.Repositories;
using Vega.Persistence.Repositories;

namespace Vega.Persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly VegaDbContext _context;

    public IVehicleRepository Vehicles { get; private set; }
    public IMakeRepository Makes { get; }
    public IModelRepository Models { get; }
    public IVehicleFeatureRepository VehicleFeatures { get; }
    public IPhotoRepository VehiclePhotos { get; }

    public UnitOfWork(VegaDbContext context)
    {
        _context = context;
        Vehicles = new VehicleRepository(_context);
        Makes = new MakeRepository(_context);
        Models = new ModelRepository(_context);
        VehicleFeatures = new VehicleFeatureRepository(_context);
        VehiclePhotos = new PhotoRepository(_context);
    }

    public int Complete() => _context.SaveChanges();
    public async Task<int> CompleteAsync() => await _context.SaveChangesAsync();

    public void Dispose() => _context.Dispose();
}
