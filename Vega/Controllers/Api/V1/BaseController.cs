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
        var errors = ModelState
            .Where(x => x.Value!.Errors.Any())
            .Select(x => new
            {
                Field = x.Key,
                Errors = x.Value!.Errors.Select(
                    e => e.ErrorMessage).ToList()
            }).ToList();

        return BadRequest(new
        {
            Message = "Invalid data provided",
            Errors = errors
        });
    }
}
