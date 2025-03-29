using AutoMapper;

using Microsoft.AspNetCore.Mvc;

using Vega.Core;

namespace Vega.Controllers.Api.V1;

using Resources.V1;

[ApiController]
[Route("api/v1/features")]
public class VehicleFeaturesController : BaseController
{
    public VehicleFeaturesController(
        IUnitOfWork unitOfWork,
        IMapper mapper
    ) : base(unitOfWork, mapper)
    {
    }

    [HttpGet]
    public ActionResult<IEnumerable<KeyValuePairResource>> GetFeatures()
    {
        var features = UnitOfWork.VehicleFeatures.GetAll();
        var list = Mapper.Map<List<KeyValuePairResource>>(features);

        return list;
    }
}
