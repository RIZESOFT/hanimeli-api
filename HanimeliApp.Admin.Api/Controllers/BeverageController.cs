using AutoMapper;
using HanimeliApp.Admin.Api.Controllers.BaseControllers;
using HanimeliApp.Application.Models;
using HanimeliApp.Application.Services;
using HanimeliApp.Domain.Dtos.Beverage;
using HanimeliApp.Domain.Entities;
using HanimeliApp.Domain.Models.Beverage;
using HanimeliApp.Domain.UnitOfWorks;
using Microsoft.AspNetCore.Mvc;

namespace HanimeliApp.Admin.Api.Controllers;

public class BeverageController : CrudBaseController<BeverageService, Beverage, BeverageModel, CreateBeverageRequest, UpdateBeverageRequest>
{
    public BeverageController(BeverageService service) : base(service)
    {
    }

    [HttpPost]
    public async Task<Result<BeverageModel>> CreateWithImage([FromForm] CreateBeverageRequest request, IFormFile imageFile)
    {
        var model = await Service.CreateWithImage(request, imageFile);
        return Result.AsSuccess(model);
    }

    [HttpPut("{id:int}")]
    public async Task<Result<BeverageModel>> UpdateWithImage(int id, [FromForm] UpdateBeverageRequest request, IFormFile imageFile)
    {
        var model = await Service.UpdateWithImage(id, request, imageFile);
        return Result.AsSuccess(model);
    }
}