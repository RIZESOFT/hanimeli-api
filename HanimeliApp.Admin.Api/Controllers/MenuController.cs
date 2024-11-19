using AutoMapper;
using HanimeliApp.Admin.Api.Controllers.BaseControllers;
using HanimeliApp.Application.Exceptions;
using HanimeliApp.Application.Models;
using HanimeliApp.Application.Services;
using HanimeliApp.Application.Utilities;
using HanimeliApp.Domain.Dtos.Menu;
using HanimeliApp.Domain.Entities;
using HanimeliApp.Domain.Models.Menu;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HanimeliApp.Admin.Api.Controllers;

public class MenuController : CrudBaseController<MenuService, Menu, MenuModel, CreateMenuRequest, UpdateMenuRequest>
{
    private readonly AzureStorageHelper _azureStorageHelper;
    public MenuController(MenuService service, AzureStorageHelper azureStorageHelper) : base(service)
    {
        _azureStorageHelper = azureStorageHelper;
    }
    
    [HttpGet]
    public override async Task<Result<List<MenuModel>>> GetList([FromQuery] int pageNumber)
    {
        var models = await Service.GetList(x => true, pageNumber, 25);
        return Result.AsSuccess(models);
    }
    
    [Authorize(Policy = "AdminPolicy")]
    [HttpPost]
    public async Task<Result<MenuModel>> CreateWithImage(IFormCollection collection, IFormFile imageFile)
    {
        var foodIds = collection["FoodIds[]"].Select(int.Parse).ToList();
        var beverageIds = collection["BeverageIds[]"].Select(int.Parse).ToList();
        var request = new CreateMenuRequest
        {
            Name = collection["Name"],
            Description = collection["Description"],
            Price = decimal.Parse(collection["Price"]),
            FoodIds = foodIds,
            BeverageIds = beverageIds
        };

        var model = await Service.CreateWithImage(request, imageFile);
        return Result.AsSuccess(model);
    }


    [Authorize(Policy = "AdminPolicy")]
    [HttpPut("{id:int}")]
    public virtual async Task<Result<MenuModel>> UpdateWithImage(int id, IFormCollection collection, IFormFile imageFile)
    {
        var foodIds = collection["FoodIds[]"].Select(int.Parse).ToList();
        var beverageIds = collection["BeverageIds[]"].Select(int.Parse).ToList();
        var request = new UpdateMenuRequest()
        {
            Name = collection["Name"],
            Description = collection["Description"],
            Price = decimal.Parse(collection["Price"]),
            FoodIds = foodIds,
            BeverageIds = beverageIds
        };
        
        var model = await Service.UpdateWithImage(id, request, imageFile);
        return Result.AsSuccess(model);
    }
    
    [Authorize(Policy = "AdminPolicy")]
    [HttpPut("{id:int}")]
    public async Task<Result> ToggleActiveStatus(int id)
    {
        await Service.ToggleActiveStatus(id);
        return Result.AsSuccess();
    }
}