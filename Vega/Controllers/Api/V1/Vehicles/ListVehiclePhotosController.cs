using AutoMapper;

using Microsoft.AspNetCore.Mvc;

using Vega.Core;
using Vega.Core.Domain;
using Vega.Resources.V1;

namespace Vega.Controllers.Api.V1.Vehicles;

[Route("/api/v1/vehicles/{id:int}/photos")]
public class ListVehiclePhotosController : BaseController
{
    public ListVehiclePhotosController(
        IUnitOfWork unitOfWork,
        IMapper mapper
    ) : base(unitOfWork, mapper)
    {
    }

    [HttpGet]
    public async Task<IEnumerable<PhotoResource>> ListPhotos([FromRoute] int id)
    {
        var photos = await UnitOfWork.VehiclePhotos.GetPhotosAsync(id);

        return Mapper.Map<List<PhotoResource>>(photos);
    }
}
