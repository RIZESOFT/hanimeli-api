using AutoMapper;
using HanimeliApp.Application.Services.Abstract;
using HanimeliApp.Domain.Dtos.Beverage;
using HanimeliApp.Domain.Entities;
using HanimeliApp.Domain.Entities.Abstract;
using HanimeliApp.Domain.Models.Beverage;
using HanimeliApp.Domain.UnitOfWorks;

namespace HanimeliApp.Application.Services;

public class BeverageService : ServiceBase<Beverage, BeverageModel, CreateBeverageRequest, UpdateBeverageRequest>
{
    public BeverageService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
    {
    }
}