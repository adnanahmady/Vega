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
        _context = context;
        _mapper = mapper;
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
    public IActionResult Create([FromBody] CreateVehicleForm data)
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
}
