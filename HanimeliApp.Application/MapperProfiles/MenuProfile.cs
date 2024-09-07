using AutoMapper;
using HanimeliApp.Domain.Dtos.Menu;
using HanimeliApp.Domain.Entities;
using HanimeliApp.Domain.Models.Food;
using HanimeliApp.Domain.Models.Menu;

namespace HanimeliApp.Application.MapperProfiles;

public class MenuProfile : Profile
{
    public MenuProfile()
    {
        CreateMap<Menu, MenuModel>().ReverseMap();
        CreateMap<CreateMenuRequest, Menu>();
        CreateMap<UpdateMenuRequest, Menu>();
    }
}