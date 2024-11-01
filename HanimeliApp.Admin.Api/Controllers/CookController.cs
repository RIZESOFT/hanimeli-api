using AutoMapper;
using HanimeliApp.Admin.Api.Controllers.BaseControllers;
using HanimeliApp.Application.Models;
using HanimeliApp.Application.Services;
using HanimeliApp.Application.Utilities;
using HanimeliApp.Domain.Dtos.Cook;
using HanimeliApp.Domain.Dtos.Food;
using HanimeliApp.Domain.Entities;
using HanimeliApp.Domain.Models.Cook;
using HanimeliApp.Domain.Models.Food;
using HanimeliApp.Domain.UnitOfWorks;
using Microsoft.AspNetCore.Mvc;

namespace HanimeliApp.Admin.Api.Controllers;

public class CookController : CrudBaseController<CookService, Cook, CookModel, CreateCookRequest, UpdateCookRequest>
{
    public CookController(CookService service, AzureStorageHelper azureStorageHelper) : base(service)
    {
    }

    [HttpPost]
    public async Task<Result<CookModel>> CreateWithImage([FromForm] CreateCookRequest request, IFormFile imageFile)
    {
        var model = await Service.CreateWithImage(request, imageFile);
        return Result.AsSuccess(model);
    }


    [HttpPut("{id:int}")]
    public virtual async Task<Result<CookModel>> UpdateWithImage(int id, [FromForm] UpdateCookRequest request, IFormFile imageFile)
    {
        var model = await Service.UpdateWithImage(id, request, imageFile);
        return Result.AsSuccess(model);
    }
} 