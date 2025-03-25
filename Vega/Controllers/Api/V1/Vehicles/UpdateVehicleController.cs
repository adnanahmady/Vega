using Microsoft.AspNetCore.Authorization;

using Vega.Core;

namespace Vega.Controllers.Api.V1.Vehicles;

using AutoMapper;

using Core.Domain;

using Forms;

using Microsoft.AspNetCore.Mvc;

using Resources.V1;

[Route("api/v1/vehicles")]
public class UpdateVehicleController : BaseController
{
    public UpdateVehicleController(
        IUnitOfWork unitOfWork,
        IMapper mapper
    ) : base(unitOfWork, mapper)
    {
    }

    [HttpPut(@"{id:int}")]
    [Authorize]
    public async Task<IActionResult> Update(
        [FromBody] VehicleForm vehicleForm,
        [FromRoute] int id)
    {
        var vehicle = await UnitOfWork
            .Vehicles
            .FindWithModelAndFeaturesAsync(id);

        if (vehicle == null)
        {
            return new NotFoundResult();
        }

        if (!ModelState.IsValid)
        {
            return ValidationResponse();
        }

        Mapper.Map(vehicleForm, vehicle);
        vehicle.VehicleFeatures = new List<VehicleFeature>();
        foreach (var i in vehicleForm.FeatureIds)
        {
            var feature = await UnitOfWork.VehicleFeatures
                .GetAsync(i);

            if (feature == null)
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

            vehicle.VehicleFeatures.Add(feature);
        }

        await UnitOfWork.CompleteAsync();

        return Ok(Mapper.Map<VehicleResource>(vehicle));
    }
}
