using System.Text.Json;
using AutoMapper;
using HanimeliApp.Domain.Dtos.User;
using HanimeliApp.Domain.Entities;
using HanimeliApp.Domain.Models.User;

namespace HanimeliApp.Application.MapperProfiles;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<CreateUserRequest, User>();
        CreateMap<User, UserModel>()
            .ForMember(o => o.OrderDays,
                opt => opt.MapFrom((u, _) =>
                    !string.IsNullOrEmpty(u.OrderDays) ? JsonSerializer.Deserialize<List<int>>(u.OrderDays) : null))
            .ForMember(o => o.OrderHours,
            opt => opt.MapFrom((u, _) =>
                !string.IsNullOrEmpty(u.OrderHours) ? JsonSerializer.Deserialize<List<string>>(u.OrderHours) : null))
            .ReverseMap();
        
        CreateMap<UpdateUserRequest, User>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        
        CreateMap<UpdateB2BUserSettingsRequest, User>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
    }
}