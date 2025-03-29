using Vega.Core;

namespace Vega.Controllers.Api.V1.Vehicles;

using AutoMapper;

using Microsoft.AspNetCore.Mvc;

using Resources.V1;

[ApiController]
[Route("api/v{version:apiVersion}/vehicles")]
[ApiVersion("1.0")]
public class ShowVehicleController : BaseController
{
    public ShowVehicleController(
        IUnitOfWork unitOfWork,
        IMapper mapper
    ) : base(unitOfWork, mapper)
    {
    }

    [HttpGet(@"{id:int}")]
    public async Task<IActionResult> Show([FromRoute] int id)
    {
        var vehicle = await UnitOfWork.Vehicles
            .FindWithModelAndFeaturesAsync(id);

        if (vehicle == null)
        {
            return new NotFoundResult();
        }

        var resource = Mapper.Map<VehicleResource>(vehicle);

        return Ok(resource);
    }
}
