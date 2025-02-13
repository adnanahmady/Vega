namespace Vega.Controllers.Api.V1;

using System.Text.Json;
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
            .Include(v => v.VehicleFeatures)
            .SingleOrDefaultAsync(v => v.Id == id);

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
            .Include(v => v.VehicleFeatures)
            .ToList();
        var list = this._mapper.Map<List<VehicleResource>>(vehicles);

        return list;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] VehicleForm data)
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
        vehicle.VehicleFeatures = new List<VehicleFeature>() { };
        foreach (var i in data.VehicleFeatureIds)
        {
            var f = await this._context.VehicleFeatures
                .SingleOrDefaultAsync(vf => vf.Id == i);

            if (f == null)
            {
                return this.BadRequest(new
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
        this._context.Vehicles.Add(vehicle);
        await this._context.SaveChangesAsync();

        return this.CreatedAtAction(
            nameof(this.GetVehicles),
            new { id = vehicle.Id },
            this._mapper.Map<VehicleResource>(vehicle));
    }

    [HttpPut(@"{id}")]
    public async Task<IActionResult> UpdateVehicle(
        [FromBody] VehicleForm data, int id)
    {
        var vehicle = await this._context
            .Vehicles
            .Include(v => v.Model)
            .Include(v => v.VehicleFeatures)
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
        vehicle.VehicleFeatures = new List<VehicleFeature>() { };
        foreach (var i in data.VehicleFeatureIds)
        {
            var f = await this._context.VehicleFeatures
                .SingleOrDefaultAsync(vf => vf.Id == i);

            if (f == null)
            {
                return this.BadRequest(new
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
