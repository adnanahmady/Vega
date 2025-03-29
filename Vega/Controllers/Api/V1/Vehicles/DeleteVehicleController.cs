using Microsoft.AspNetCore.Authorization;

using Vega.Core;

namespace Vega.Controllers.Api.V1.Vehicles;

using AutoMapper;

using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/v{version:apiVersion}/vehicles")]
[ApiVersion("1.0")]
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
