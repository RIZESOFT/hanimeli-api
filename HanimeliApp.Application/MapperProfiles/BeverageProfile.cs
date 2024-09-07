using AutoMapper;
using HanimeliApp.Domain.Dtos.Beverage;
using HanimeliApp.Domain.Entities;
using HanimeliApp.Domain.Models.Beverage;

namespace HanimeliApp.Application.MapperProfiles;

public class BeverageProfile : Profile
{
    public BeverageProfile()
    {
        CreateMap<Beverage, BeverageModel>().ReverseMap();
        CreateMap<CreateBeverageRequest, Beverage>();
        CreateMap<UpdateBeverageRequest, Beverage>();
    }
}