using AutoMapper;
using HanimeliApp.Admin.Api.Controllers.BaseControllers;
using HanimeliApp.Application.Services;
using HanimeliApp.Domain.Dtos.Beverage;
using HanimeliApp.Domain.Entities;
using HanimeliApp.Domain.Models.Beverage;
using HanimeliApp.Domain.UnitOfWorks;

namespace HanimeliApp.Admin.Api.Controllers;

public class BeverageController : CrudBaseController<BeverageService, Beverage, BeverageModel, CreateBeverageRequest, UpdateBeverageRequest>
{
    public BeverageController(BeverageService service) : base(service)
    {
    }
}