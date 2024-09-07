using AutoMapper;
using HanimeliApp.Application.Services.Abstract;
using HanimeliApp.Domain.Dtos.Food;
using HanimeliApp.Domain.Entities;
using HanimeliApp.Domain.Models.Food;
using HanimeliApp.Domain.UnitOfWorks;

namespace HanimeliApp.Application.Services;

public class FoodService : ServiceBase<Food, FoodModel, CreateFoodRequest, UpdateFoodRequest>
{
    public FoodService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
    {
    }
}