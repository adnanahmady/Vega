using Vega.Core;

namespace Vega.Controllers.Api.V1;

using AutoMapper;

using Core.Domain;

using Forms;

using Microsoft.AspNetCore.Mvc;

using Resources.V1;

[Route("api/v1/vehicles")]
public class VehiclesController : Controller
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public VehiclesController(
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    [HttpGet]
    public ActionResult<IEnumerable<VehicleResource>> Index()
    {
        var vehicles = _unitOfWork
            .Vehicles
            .GetAllWithModelAndFeatures();
        var list = _mapper.Map<List<VehicleResource>>(vehicles);

        return list;
    }

    [HttpGet(@"{id}")]
    public async Task<IActionResult> Show(int id)
    {
        var vehicle = await _unitOfWork.Vehicles
            .FindWithModelAndFeaturesAsync(id);

        if (vehicle == null)
        {
            return new NotFoundResult();
        }

        var resource = _mapper.Map<VehicleResource>(vehicle);

        return Ok(resource);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] VehicleForm formData)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState
                .Where(x => x.Value.Errors.Any())
                .Select(x => new
                {
                    Field = x.Key,
                    Errors = x.Value.Errors.Select(
                        e => e.ErrorMessage).ToList()
                })
                .ToList();

            return BadRequest(new
            {
                Message = "Invalid data provided",
                Errors = errors
            });
        }

        var model = await _unitOfWork.Models.GetAsync(formData.ModelId);
        if (model == null)
        {
            ModelState.AddModelError("ModelId", "Invalid model id.");

            return BadRequest(new
            {
                Message = "Invalid data provided",
                Errors = new[] { new
                {
                    Field = ModelState.First().Key,
                    Errors = ModelState.First().Value?.Errors.Select(
                        e => e.ErrorMessage).ToList()
                } }
            });
        }

        var vehicle = _mapper.Map<Vehicle>(formData);
        vehicle.VehicleFeatures = new List<VehicleFeature>();

        foreach (var i in formData.FeatureIds)
        {
            var f = await _unitOfWork.VehicleFeatures.GetAsync(i);

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

        _unitOfWork.Vehicles.Add(vehicle);
        await _unitOfWork.CompleteAsync();

        return CreatedAtAction(
            nameof(Index),
            null,
            _mapper.Map<VehicleResource>(vehicle));
    }

    [HttpPut(@"{id}")]
    public async Task<IActionResult> Update(
        [FromBody] VehicleForm vehicleForm, int id)
    {
        var vehicle = await _unitOfWork
            .Vehicles
            .FindWithModelAndFeaturesAsync(id);

        if (vehicle == null)
        {
            return new NotFoundResult();
        }

        if (!ModelState.IsValid)
        {
            var errors = ModelState
                .Where(x => x.Value.Errors.Any())
                .Select(x => new
                {
                    Field = x.Key,
                    Errors = x.Value.Errors.Select(
                        e => e.ErrorMessage).ToList()
                }).ToList();

            return BadRequest(new
            {
                Message = "Invalid data provided",
                Errors = errors
            });
        }

        _mapper.Map(vehicleForm, vehicle);
        vehicle.VehicleFeatures = new List<VehicleFeature>();
        foreach (var i in vehicleForm.FeatureIds)
        {
            var feature = await _unitOfWork.VehicleFeatures
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

        await _unitOfWork.CompleteAsync();

        return Ok(_mapper.Map<VehicleResource>(vehicle));
    }

    [HttpDelete(@"{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _unitOfWork.Vehicles
            .ExecuteDeleteAsync(id);

        return result == 0 ? NotFound() : NoContent();
    }
}
