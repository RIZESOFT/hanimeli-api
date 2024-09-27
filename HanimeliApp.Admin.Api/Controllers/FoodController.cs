using AutoMapper;
using HanimeliApp.Admin.Api.Controllers.BaseControllers;
using HanimeliApp.Application.Services;
using HanimeliApp.Domain.Dtos.Food;
using HanimeliApp.Domain.Entities;
using HanimeliApp.Domain.Models.Food;
using HanimeliApp.Domain.UnitOfWorks;

namespace HanimeliApp.Admin.Api.Controllers;

public class FoodController : CrudBaseController<FoodService, Food, FoodModel, CreateFoodRequest, UpdateFoodRequest>
{
    public FoodController(FoodService service) : base(service)
    {
    }
}