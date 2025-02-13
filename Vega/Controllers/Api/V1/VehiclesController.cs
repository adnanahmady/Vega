namespace Vega.Controllers.Api.V1;

using AutoMapper;
using Domain;
using Forms;
using Microsoft.AspNetCore.Http.HttpResults;
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
        this._context = context;
        this._mapper = mapper;
    }

    [HttpGet(@"{id}")]
    public async Task<IActionResult> ShowVehicle(int id)
    {
        var vehicle = await this._context.Vehicles
            .Include(v => v.Model)
            .Include(v => v.VehicleFeature)
            .SingleOrDefaultAsync( v => v.Id == id);

        if (vehicle == null)
        {
            return new NotFoundResult();
        }

        var resource = this._mapper.Map<VehicleResource>(vehicle);

        return this.Ok(resource);
    }

    [HttpGet]
    public ActionResult<IEnumerable<VehicleResource>> GetVehicles()
    {
        var vehicles = this._context
            .Vehicles
            .Include(v => v.Model)
            .Include(v => v.VehicleFeature)
            .ToList();
        var list = this._mapper.Map<List<VehicleResource>>(vehicles);

        return list;
    }

    [HttpPost]
    public IActionResult Create([FromBody] VehicleForm data)
    {
        if (!this.ModelState.IsValid)
        {
            var errors = this.ModelState
                .Where(x => x.Value.Errors.Any())
                .Select(x => new
                {
                    Field = x.Key,
                    Errors = x.Value.Errors.Select(
                        e => e.ErrorMessage).ToList()
                })
                .ToList();

            return this.BadRequest(new
            {
                Message = "Invalid data provided",
                Errors = errors
            });
        }
        var vehicle = this._mapper.Map<Vehicle>(data);
        this._context.Vehicles.Add(vehicle);
        this._context.SaveChanges();

        return CreatedAtAction(
            nameof(this.GetVehicles),
            new { id = vehicle.Id },
            vehicle);
    }

    [HttpPut(@"{id}")]
    public async Task<IActionResult> UpdateVehicle(
        [FromBody] VehicleForm data, int id)
    {
        var vehicle = await this._context
            .Vehicles
            .Include(v => v.Model)
            .Include(v => v.VehicleFeature)
            .SingleOrDefaultAsync(v => v.Id == id);

        if (vehicle == null)
        {
            return new NotFoundResult();
        }

        if (!this.ModelState.IsValid)
        {
            var errors = this.ModelState
                .Where(x => x.Value.Errors.Any())
                .Select(x => new
                {
                    Field = x.Key,
                    Errors = x.Value.Errors.Select(
                        e => e.ErrorMessage).ToList()
                }).ToList();

            return this.BadRequest(new
            {
                Message = "Invalid data provided",
                Errors = errors
            });
        }

        this._mapper.Map<VehicleForm, Vehicle>(data, vehicle);
        await this._context.SaveChangesAsync();

        return this.Ok(this._mapper.Map<VehicleResource>(vehicle));
    }

    [HttpDelete(@"{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await this._context.Vehicles
            .Where(v => v.Id == id)
            .ExecuteDeleteAsync();

        if (result == 0)
        {
            return this.NotFound();
        }

        return this.NoContent();
    }
}
