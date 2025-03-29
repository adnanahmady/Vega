using Microsoft.AspNetCore.Authorization;

using Vega.Core;

namespace Vega.Controllers.Api.V1.Vehicles;

using AutoMapper;

using Core.Domain;

using Forms;

using Microsoft.AspNetCore.Mvc;

using Resources.V1;

[ApiController]
[Route("api/v1/vehicles")]
public class CreateVehicleController : BaseController
{
    public CreateVehicleController(
        IUnitOfWork unitOfWork,
        IMapper mapper
    ) : base(unitOfWork, mapper)
    {
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create([FromBody] VehicleForm data)
    {
        var model = await UnitOfWork.Models.GetAsync(data.ModelId);
        if (model == null)
        {
            ModelState.AddModelError("ModelId", "Invalid model id.");
            return ValidationResponse();
        }

        var vehicle = Mapper.Map<Vehicle>(data);
        vehicle.VehicleFeatures = new List<VehicleFeature>();

        foreach (var i in data.FeatureIds)
        {
            var f = await UnitOfWork.VehicleFeatures.GetAsync(i);

            if (f == null)
            {
                return BadRequest(new
                {
                    Message = "Invalid data provided",
                    Errors = new
                    {
                        Field = "VehicleFeatureIds",
                        Errors = new[] { "VehicleFeature With Id \"{i}\" not found." }
                    }
                });
            }
            vehicle.VehicleFeatures.Add(f);
        }

        UnitOfWork.Vehicles.Add(vehicle);
        await UnitOfWork.CompleteAsync();

        return CreatedAtAction(
            nameof(Index),
            null,
            Mapper.Map<VehicleResource>(vehicle));
    }
}
