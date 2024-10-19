using AutoMapper;
using HanimeliApp.Domain.Dtos.Address;
using HanimeliApp.Domain.Dtos.User;
using HanimeliApp.Domain.Entities;
using HanimeliApp.Domain.Models.Address;

namespace HanimeliApp.Application.MapperProfiles;

public class AddressProfile : Profile
{
    public AddressProfile()
    {
        CreateMap<CreateAddressRequest, Address>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        CreateMap<Address, AddressModel>()
            .ForMember(o => o.State, opts => opts.MapFrom(src => src.State != null ? src.State.Name : ""))
            .ForMember(o => o.City, opts => opts.MapFrom(src => src.City != null ? src.City.Name : ""))
            .ForMember(o => o.Country, opts => opts.MapFrom(src => src.Country != null ? src.Country.Name : ""))
            .ReverseMap();
    }
}