using Vega.Core.Domain;
using Vega.Core.Repositories;

namespace Vega.Persistence.Repositories;

public class ModelRepository : Repository<Model>, IModelRepository
{
    public ModelRepository(VegaDbContext context) : base(context)
    {
    }
}
