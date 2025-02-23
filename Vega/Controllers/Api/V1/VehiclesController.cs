namespace Vega.Controllers.Api.V1;

using AutoMapper;

using Domain;

using Forms;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Models;

using Resources.V1;

[Route("api/v1/vehicles")]
public class VehiclesController : Controller
{
    private readonly VegaDbContext _context;
    private readonly IMapper _mapper;

    public VehiclesController(
        VegaDbContext context,
        IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpGet]
    public ActionResult<IEnumerable<VehicleResource>> Index()
    {
        var vehicles = _context
            .Vehicles
            .Include(v => v.Model)
            .Include(v => v.VehicleFeatures)
            .ToList();
        var list = _mapper.Map<List<VehicleResource>>(vehicles);

        return list;
    }

    [HttpGet(@"{id}")]
    public async Task<IActionResult> Show(int id)
    {
        var vehicle = await _context.Vehicles
            .Include(v => v.Model)
            .Include(v => v.VehicleFeatures)
            .SingleOrDefaultAsync(v => v.Id == id);

        if (vehicle == null)
        {
            return new NotFoundResult();
        }

        var resource = _mapper.Map<VehicleResource>(vehicle);

        return Ok(resource);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] VehicleForm data)
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

        var vehicle = _mapper.Map<Vehicle>(data);
        vehicle.VehicleFeatures = new List<VehicleFeature>();

        foreach (var i in data.FeatureIds)
        {
            var f = await _context.VehicleFeatures
                .SingleOrDefaultAsync(vf => vf.Id == i);

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

        _context.Vehicles.Add(vehicle);
        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(Index),
            null,
            _mapper.Map<VehicleResource>(vehicle));
    }

    [HttpPut(@"{id}")]
    public async Task<IActionResult> Update(
        [FromBody] VehicleForm vehicleForm, int id)
    {
        var vehicle = await _context
            .Vehicles
            .Include(v => v.Model)
            .Include(v => v.VehicleFeatures)
            .SingleOrDefaultAsync(v => v.Id == id);

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
            var f = await _context.VehicleFeatures
                .SingleOrDefaultAsync(vf => vf.Id == i);

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

        await _context.SaveChangesAsync();

        return Ok(_mapper.Map<VehicleResource>(vehicle));
    }

    [HttpDelete(@"{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _context.Vehicles
            .Where(v => v.Id == id)
            .ExecuteDeleteAsync();

        if (result == 0)
        {
            return NotFound();
        }

        return NoContent();
    }
}
