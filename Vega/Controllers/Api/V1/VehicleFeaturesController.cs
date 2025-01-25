using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Vega.Domain;
using Vega.Dtos.V1;

namespace Vega.Controllers.Api.V1;

public class VehicleFeaturesController
{
    private readonly VegaDbContext _context;
    private readonly IMapper _mapper;

    public VehicleFeaturesController(
        VegaDbContext context,
        IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpGet(@"api/v1/features")]
    public ActionResult<IEnumerable<VehicleFeatureDto>> GetFeatures()
    {
        var features = _context.VehicleFeatures.ToList();
        var list = _mapper.Map<List<VehicleFeatureDto>>(features);

        return list;
    }
}
