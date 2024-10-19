using AutoMapper;
using HanimeliApp.Admin.Api.Controllers.BaseControllers;
using HanimeliApp.Application.Models;
using HanimeliApp.Application.Services;
using HanimeliApp.Domain.Dtos.Cook;
using HanimeliApp.Domain.Entities;
using HanimeliApp.Domain.Models.Cook;
using HanimeliApp.Domain.UnitOfWorks;
using Microsoft.AspNetCore.Mvc;

namespace HanimeliApp.Admin.Api.Controllers;

public class CookController : CrudBaseController<CookService, Cook, CookModel, CreateCookRequest, UpdateCookRequest>
{
    public CookController(CookService service) : base(service)
    {
    }
} 