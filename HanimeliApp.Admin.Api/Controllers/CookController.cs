﻿using AutoMapper;
using HanimeliApp.Application.Services;
using HanimeliApp.Domain.Dtos.Cook;
using HanimeliApp.Domain.Entities;
using HanimeliApp.Domain.Models.Cook;
using HanimeliApp.Domain.UnitOfWorks;

namespace HanimeliApp.Admin.Api.Controllers;

public class CookController : CrudBaseController<CookService, Cook, CookModel, CreateCookRequest, UpdateCookRequest>
{
    public CookController(CookService service) : base(service)
    {
    }
}