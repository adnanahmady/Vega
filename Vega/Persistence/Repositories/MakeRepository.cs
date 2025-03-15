using Microsoft.EntityFrameworkCore;

using Vega.Core.Domain;
using Vega.Core.Repositories;

namespace Vega.Persistence.Repositories;

public class MakeRepository : Repository<Make>, IMakeRepository
{
    private VegaDbContext VegaDbContext => (Context as VegaDbContext)!;

    public MakeRepository(VegaDbContext context) : base(context)
    {
    }

    public IEnumerable<Make> GetWithModels() =>
        VegaDbContext.Makes
            .Include(m => m.Models)
            .ToList();
}
