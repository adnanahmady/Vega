using Vega.Core.Domain;

namespace Vega.Profiles;

using AutoMapper;

using Forms;

using Resources.V1;


public class MappingProfile : Profile
{
    public MappingProfile()
    {
        MapBiDirectionalModelResource();
        MapFromRequestFormToDomainModel();
        CreateMap<Vehicle, VehicleResource>()
            .ForMember(vr => vr.Contact, opt => opt.MapFrom(v => new ContactResource()
            {
                Name = v.ContactName,
                Email = v.ContactEmail,
                Phone = v.ContactPhone
            }));
    }

    private void MapBiDirectionalModelResource()
    {
        AddMap<Make, MakeResource>();
        AddMap<Model, ModelResource>();
        AddMap<VehicleFeature, VehicleFeatureResource>();
    }

    private void MapFromRequestFormToDomainModel() =>
        CreateMap<VehicleForm, Vehicle>()
            .ForMember(v => v.Id, opt => opt.Ignore())
            .ForMember(
                v => v.ContactEmail,
                opt => opt.MapFrom(f => f.Contact.Email))
            .ForMember(
                v => v.ContactName,
                opt => opt.MapFrom(f => f.Contact.Name))
            .ForMember(
                v => v.ContactPhone,
                opt => opt.MapFrom(f => f.Contact.Phone));

    private void AddMap<TModel, TDto>()
    {
        CreateMap<TModel, TDto>();
        CreateMap<TDto, TModel>().ForMember(
            "Id",
            opt => opt.Ignore()
        );
    }
}
