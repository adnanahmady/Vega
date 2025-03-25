using Microsoft.AspNetCore.Authorization;

using Vega.Core;

namespace Vega.Controllers.Api.V1.Vehicles;

using AutoMapper;

using Microsoft.AspNetCore.Mvc;

[Route("api/v1/vehicles")]
public class DeleteVehicleController : BaseController
{
    public DeleteVehicleController(
        IUnitOfWork unitOfWork,
        IMapper mapper
    ) : base(unitOfWork, mapper)
    {
    }

    [HttpDelete(@"{id:int}")]
    [Authorize]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        var result = await UnitOfWork.Vehicles
            .ExecuteDeleteAsync(id);

        return result == 0 ? NotFound() : NoContent();
    }
}
