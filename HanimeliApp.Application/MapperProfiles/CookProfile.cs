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
            .ForMember(o => o.Name, opt =>
            {
                opt.Condition(src => !string.IsNullOrEmpty(src.Nickname));
                opt.MapFrom(o => o.Nickname);
            })
            .ForPath(o => o.User.LastName, opt =>
            {
                opt.Condition(src => !string.IsNullOrEmpty(src.SourceMember));
                opt.MapFrom(o => o.LastName);
            })
            .ForPath(o => o.User.Email, opt =>
            {
                opt.Condition(src => !string.IsNullOrEmpty(src.SourceMember));
                opt.MapFrom(o => o.Email);
            })
            .ForPath(o => o.User.Phone, opt =>
            {
                opt.Condition(src => !string.IsNullOrEmpty(src.SourceMember));

                opt.MapFrom(o => o.Phone);
            })
            .ForPath(o => o.User.Name, opt =>
            {
                opt.Condition(src => !string.IsNullOrEmpty(src.SourceMember));

                opt.MapFrom(o => o.Name);
            })
            .ForPath(o => o.User.Id, opt => opt.Ignore())
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
    }
}