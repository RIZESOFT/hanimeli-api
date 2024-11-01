using AutoMapper;
using HanimeliApp.Domain.Dtos.Cook;
using HanimeliApp.Domain.Entities;
using HanimeliApp.Domain.Models.Cook;

namespace HanimeliApp.Application.MapperProfiles;

public class CookProfile : Profile
{
    public CookProfile()
    {
        CreateMap<Cook, CookModel>().ReverseMap();
        CreateMap<CreateCookRequest, Cook>()
            .ForMember(o => o.Name, opt => opt.MapFrom(o => o.Nickname));
        CreateMap<UpdateCookRequest, Cook>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
    }
}