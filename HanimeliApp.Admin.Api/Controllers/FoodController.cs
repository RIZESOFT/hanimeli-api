using AutoMapper;
using HanimeliApp.Domain.Dtos.Food;
using HanimeliApp.Domain.Entities;
using HanimeliApp.Domain.Models.Food;
using HanimeliApp.Domain.UnitOfWorks;

namespace HanimeliApp.Admin.Api.Controllers;

public class FoodController : CrudBaseController<Food, FoodModel, CreateFoodRequest, UpdateFoodRequest>
{
    public FoodController(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
    {
    }
}