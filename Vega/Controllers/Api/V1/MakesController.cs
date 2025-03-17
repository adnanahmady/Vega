using AutoMapper;

using Microsoft.AspNetCore.Mvc;

using Vega.Core;

namespace Vega.Controllers.Api.V1;

using Resources.V1;

[Route("api/v1/makes")]
[ApiController]
public class MakesController : BaseController
{
    public MakesController(
        IUnitOfWork unitOfWork,
        IMapper mapper
    ) : base(unitOfWork, mapper)
    {
    }

    [HttpGet]
    public ActionResult<IEnumerable<MakeResource>> GetMakes()
    {
        var makes = UnitOfWork.Makes.GetWithModels();
        var list = Mapper.Map<List<MakeResource>>(makes);

        return list;
    }
}
