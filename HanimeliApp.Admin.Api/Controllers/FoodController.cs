using AutoMapper;
using HanimeliApp.Admin.Api.Controllers.BaseControllers;
using HanimeliApp.Application.Models;
using HanimeliApp.Application.Services;
using HanimeliApp.Application.Utilities;
using HanimeliApp.Domain.Dtos.Food;
using HanimeliApp.Domain.Entities;
using HanimeliApp.Domain.Models.Food;
using HanimeliApp.Domain.UnitOfWorks;
using Microsoft.AspNetCore.Mvc;

namespace HanimeliApp.Admin.Api.Controllers;

public class FoodController : CrudBaseController<FoodService, Food, FoodModel, CreateFoodRequest, UpdateFoodRequest>
{
    private readonly AzureStorageHelper _azureStorageHelper;

    public FoodController(FoodService service, AzureStorageHelper azureStorageHelper) : base(service)
    {
        _azureStorageHelper = azureStorageHelper;
    }

    [HttpPost]
    public async Task<Result<FoodModel>> CreateWithImage([FromForm] CreateFoodRequest request, IFormFile imageFile)
    {
        var model = await Service.CreateWithImage(request, imageFile);
        return Result.AsSuccess(model);
    }


    [HttpPut("{id:int}")]
    public virtual async Task<Result<FoodModel>> UpdateWithImage(int id, [FromForm] UpdateFoodRequest request, IFormFile imageFile)
    {
        var model = await Service.UpdateWithImage(id, request, imageFile);
        return Result.AsSuccess(model);
    }
}
