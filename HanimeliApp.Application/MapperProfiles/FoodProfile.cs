using AutoMapper;
using HanimeliApp.Domain.Dtos.Food;
using HanimeliApp.Domain.Entities;
using HanimeliApp.Domain.Models.Food;

namespace HanimeliApp.Application.MapperProfiles;

public class FoodProfile : Profile
{
    public FoodProfile()
    {
        CreateMap<Food, FoodModel>().ReverseMap();
        CreateMap<CreateFoodRequest, Food>();
        CreateMap<UpdateFoodRequest, Food>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null)); ;
    }
}