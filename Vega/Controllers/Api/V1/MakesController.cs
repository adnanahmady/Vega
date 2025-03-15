using AutoMapper;

using Microsoft.AspNetCore.Mvc;

using Vega.Core;

namespace Vega.Controllers.Api.V1;

using Resources.V1;

[Route("api/v1/makes")]
[ApiController]
public class MakesController : Controller
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public MakesController(
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    [HttpGet]
    public ActionResult<IEnumerable<MakeResource>> GetMakes()
    {
        var makes = _unitOfWork.Makes.GetWithModels();
        var list = _mapper.Map<List<MakeResource>>(makes);

        return list;
    }
}
