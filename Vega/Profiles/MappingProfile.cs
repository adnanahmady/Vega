using AutoMapper;
using Vega.Dtos.V1;
using Vega.Models;

namespace Vega.Profiles;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        AddMap<Make, MakeDto>();
        AddMap<Model, ModelDto>();
        AddMap<VehicleFeature, VehicleFeatureDto>();
    }

    private void AddMap<TModel, TDto>()
    {
        CreateMap<TModel, TDto>();
        CreateMap<TDto, TModel>();
    }
}
