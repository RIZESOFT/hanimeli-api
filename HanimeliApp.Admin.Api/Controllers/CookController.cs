using AutoMapper;
using HanimeliApp.Domain.Dtos.Cook;
using HanimeliApp.Domain.Entities;
using HanimeliApp.Domain.Models.Cook;
using HanimeliApp.Domain.UnitOfWorks;

namespace HanimeliApp.Admin.Api.Controllers;

public class CookController : CrudBaseController<Cook, CookModel, CreateCookRequest, UpdateCookRequest>
{
    public CookController(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
    {
    }
}