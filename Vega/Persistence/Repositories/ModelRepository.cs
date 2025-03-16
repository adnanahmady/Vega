using Microsoft.EntityFrameworkCore;

using Vega.Core.Domain;
using Vega.Core.Repositories;

namespace Vega.Persistence.Repositories;

public class ModelRepository : Repository<Model>, IModelRepository
{
    private VegaDbContext VegaDbContext => (Context as VegaDbContext)!;

    public ModelRepository(VegaDbContext context) : base(context)
    {
    }

    public override async Task<Model?> GetAsync(int id) => await VegaDbContext
        .Models
        .Include(m => m.Make)
        .SingleOrDefaultAsync(m => m.Id == id);
}
