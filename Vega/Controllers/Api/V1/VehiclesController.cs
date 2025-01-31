namespace Vega.Controllers.Api.V1;

using AutoMapper;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using Resources.V1;

public class VehiclesController
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

    [HttpGet(@"/api/v1/vehicles")]
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
}
