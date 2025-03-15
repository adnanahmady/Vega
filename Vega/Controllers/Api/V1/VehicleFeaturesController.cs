using AutoMapper;

using Microsoft.AspNetCore.Mvc;

using Vega.Core;

namespace Vega.Controllers.Api.V1;

using Resources.V1;

[Route("api/v1/features")]
public class VehicleFeaturesController : Controller
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public VehicleFeaturesController(
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    [HttpGet]
    public ActionResult<IEnumerable<KeyValuePairResource>> GetFeatures()
    {
        var features = _unitOfWork.VehicleFeatures.GetAll();
        var list = _mapper.Map<List<KeyValuePairResource>>(features);

        return list;
    }
}
