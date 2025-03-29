using Vega.Core;

namespace Vega.Controllers.Api.V1;

using AutoMapper;

using Microsoft.AspNetCore.Mvc;

public abstract class BaseController : Controller
{
    protected readonly IUnitOfWork UnitOfWork;
    protected readonly IMapper Mapper;

    protected BaseController(IUnitOfWork unitOfWork, IMapper mapper)
    {
        UnitOfWork = unitOfWork;
        Mapper = mapper;
    }

    protected IActionResult ValidationResponse()
    {
        var errors = new Dictionary<string, object>();
        var states = ModelState.Where(x => x.Value!.Errors.Any());

        foreach (var x in states)
        {
            errors.Add(
                x.Key,
                x.Value!.Errors.Select(e => e.ErrorMessage).ToList()
            );
        }

        return BadRequest(new
        {
            Message = "Invalid data provided",
            Errors = errors
        });
    }
}
