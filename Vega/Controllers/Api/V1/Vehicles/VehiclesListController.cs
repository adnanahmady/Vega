using Vega.Core;
using Vega.Core.Domain;
using Vega.Core.QueryFilters;
using Vega.Resources.Support;

namespace Vega.Controllers.Api.V1.Vehicles;

using AutoMapper;

using Microsoft.AspNetCore.Mvc;

using Resources.V1;

[ApiController]
[Route("api/v{version:apiVersion}/vehicles")]
[ApiVersion("1.0")]
public class VehiclesListController : BaseController
{
    private readonly IQueryParamProcessor<Vehicle> _paramProcessor;

    public VehiclesListController(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IVehiclesListParamProcessor paramProcessor
    ) : base(unitOfWork, mapper) => _paramProcessor = paramProcessor;

    [HttpGet]
    public PageResult<VehicleResource> Index(
        [FromQuery] int pageNumber, [FromQuery] int pageSize)
    {
        var pageIndex = pageNumber > 0 ? pageNumber - 1 : 0;
        pageSize = pageSize < 1 ? 15 : pageSize;

        var result = UnitOfWork.Vehicles
            .QueryParamProcessor(_paramProcessor)
            .GetAll(skip: pageIndex, take: pageSize);

        var list = Mapper.Map<List<VehicleResource>>(result.Items);

        return new PageResult<VehicleResource>()
        {
            Items = list,
            CurrentPage = pageNumber,
            PageSize = pageSize,
            TotalRecords = result.Count,
        };
    }
}
