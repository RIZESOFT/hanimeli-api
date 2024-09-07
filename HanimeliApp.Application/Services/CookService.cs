using AutoMapper;
using HanimeliApp.Application.Services.Abstract;
using HanimeliApp.Domain.Dtos.Beverage;
using HanimeliApp.Domain.Dtos.Cook;
using HanimeliApp.Domain.Entities;
using HanimeliApp.Domain.Models.Beverage;
using HanimeliApp.Domain.Models.Cook;
using HanimeliApp.Domain.UnitOfWorks;

namespace HanimeliApp.Application.Services;

public class CookService : ServiceBase<Cook, CookModel, CreateCookRequest, UpdateCookRequest>
{
    public CookService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
    {
    }
}