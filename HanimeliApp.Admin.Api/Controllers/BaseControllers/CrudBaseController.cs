﻿using HanimeliApp.Application.Models;
using HanimeliApp.Application.Services.Abstract;
using HanimeliApp.Domain.Entities.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HanimeliApp.Admin.Api.Controllers.BaseControllers;

public class CrudBaseController<TService, TEntity, TModel, TCreateRequest, TUpdateRequest> : BaseController
    where TService : ServiceBase<TEntity, TModel, TCreateRequest, TUpdateRequest>
    where TEntity : BaseEntity<int>
{
    public readonly TService Service;

    public CrudBaseController(TService service)
    {
        Service = service;
    }

    [Authorize(Policy = "AdminPolicy")]
    [HttpGet("{id}")]
    public async Task<Result<TModel>> GetById(int id)
    {
        var model = await Service.GetById(id);
        return Result.AsSuccess(model);
    }

    [Authorize(Policy = "AdminPolicy")]
    [HttpGet]
    public virtual async Task<Result<List<TModel>>> GetList([FromQuery] int pageNumber)
    {
        var models = await Service.GetList(x => true, pageNumber, 25);
        return Result.AsSuccess(models);
    }

    [Authorize(Policy = "AdminPolicy")]
    [HttpPost]
    public virtual async Task<Result<TModel>> Create([FromBody] TCreateRequest request)
    {
        var model = await Service.Create(request);
        return Result.AsSuccess(model);
    }

    [Authorize(Policy = "AdminPolicy")]
    [HttpPut("{id:int}")]
    public virtual async Task<Result<TModel>> Update(int id, [FromBody] TUpdateRequest request)
    {
        var model = await Service.Update(id, request);
        return Result.AsSuccess(model);
    }

    [Authorize(Policy = "AdminPolicy")]
    [HttpDelete("{id:int}")]
    public async Task<Result> Delete(int id)
    {
        await Service.Delete(id);
        return Result.AsSuccess();
    }
}
