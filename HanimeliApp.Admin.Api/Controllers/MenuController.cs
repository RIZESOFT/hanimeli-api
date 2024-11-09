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

    [Authorize(Policy = "AdminPolicy")]
    [HttpPost]
    public async Task<Result<MenuModel>> CreateWithImage([FromForm] CreateMenuRequest request, IFormFile imageFile)
    {
        var model = await Service.CreateWithImage(request, imageFile);
        return Result.AsSuccess(model);
    }


    [Authorize(Policy = "AdminPolicy")]
    [HttpPut("{id:int}")]
    public virtual async Task<Result<MenuModel>> UpdateWithImage(int id, [FromForm] UpdateMenuRequest request, IFormFile imageFile)
    {
        var model = await Service.UpdateWithImage(id, request, imageFile);
        return Result.AsSuccess(model);
    }
}