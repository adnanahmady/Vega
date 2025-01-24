using Microsoft.AspNetCore.Mvc;
using Vega.Dtos.V1;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Vega.Domain;

namespace Vega.Controllers.Api.V1;

[ApiController]
public class MakesController
{
    private readonly VegaDbContext _context;
    private readonly IMapper _mapper;

    public MakesController(VegaDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpGet("api/v1/makes")]
    public ActionResult<IEnumerable<MakeDto>> GetMakes()
    {
        var makes = _context.Makes
            .Include(m => m.Models)
            .ToList();
        var list = _mapper.Map<List<MakeDto>>(makes);

        return list;
    }
}