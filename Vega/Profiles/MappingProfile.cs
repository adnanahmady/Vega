using AutoMapper;
using Vega.Models;

namespace Vega.Profiles;

using AutoMapper.Internal;
using Forms;
using Resources.V1;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        AddMap<Make, MakeResource>();
        AddMap<Model, ModelResource>();
        AddMap<VehicleFeature, VehicleFeatureResource>();
        AddMap<Vehicle, VehicleResource>();
        CreateMap<VehicleForm, Vehicle>();
    }

    private void AddMap<TModel, TDto>()
    {
        CreateMap<TModel, TDto>();
        CreateMap<TDto, TModel>().ForMember(
            "Id",
            opt => opt.Ignore()
        );
    }
}
