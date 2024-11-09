using AutoMapper;
using HanimeliApp.Domain.Dtos.Cook;
using HanimeliApp.Domain.Entities;
using HanimeliApp.Domain.Models.Cook;

namespace HanimeliApp.Application.MapperProfiles;

public class CookProfile : Profile
{
    public CookProfile()
    {
        CreateMap<Cook, CookModel>()
            .ForMember(o => o.LastName, opt => opt.MapFrom(o => o.User.LastName))
            .ForMember(o => o.Email, opt => opt.MapFrom(o => o.User.Email))
            .ForMember(o => o.Phone, opt => opt.MapFrom(o => o.User.Phone))
            .ForMember(o => o.Name, opt => opt.MapFrom(o => o.User.Name))
            .ForMember(o => o.Nickname, opt => opt.MapFrom(o => o.Name))
            .ForMember(o => o.UserId, opt => opt.MapFrom(o => o.User.Id))
            .ReverseMap();
        CreateMap<CreateCookRequest, Cook>()
            .ForMember(o => o.Name, opt => opt.MapFrom(o => o.Nickname));
        CreateMap<UpdateCookRequest, Cook>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
    }
}