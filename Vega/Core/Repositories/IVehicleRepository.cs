using Vega.Core.Domain;

namespace Vega.Core.Repositories;

public interface IVehicleRepository : IRepository<Vehicle>
{
    Vehicle? FindWithModelAndFeatures(int id);
    Task<Vehicle?> FindWithModelAndFeaturesAsync(int id);


    IEnumerable<Vehicle> GetAllWithModelAndFeatures();

    IEnumerable<Vehicle> GetPaginatedWithModelAndFeatures(int pageIndex, int pageSize);
    Task<IEnumerable<Vehicle>> GetPaginatedWithModelAndFeaturesAsync(int pageIndex, int pageSize);

    Task<int> ExecuteDeleteAsync(int id);
}
