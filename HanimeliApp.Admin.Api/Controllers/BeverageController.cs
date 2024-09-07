using AutoMapper;
using HanimeliApp.Domain.Dtos.Beverage;
using HanimeliApp.Domain.Entities;
using HanimeliApp.Domain.Models.Beverage;
using HanimeliApp.Domain.UnitOfWorks;

namespace HanimeliApp.Admin.Api.Controllers;

public class BeverageController : CrudBaseController<Beverage, BeverageModel, CreateBeverageRequest, UpdateBeverageRequest>
{
    public BeverageController(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
    {
    }
}