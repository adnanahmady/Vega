using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Vega.Domain;

namespace Vega.Controllers.Api.V1;

using Models;
using Resources.V1;

[Route("api/v1/makes")]
[ApiController]
public class MakesController : Controller
{
    private readonly VegaDbContext _context;
    private readonly IMapper _mapper;

    public MakesController(VegaDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpGet]
    public ActionResult<IEnumerable<MakeResource>> GetMakes()
    {
        var makes = _context.Makes
            .Include(m => m.Models)
            .ToList();
        var list = _mapper.Map<List<MakeResource>>(makes);

        return list;
    }
}
