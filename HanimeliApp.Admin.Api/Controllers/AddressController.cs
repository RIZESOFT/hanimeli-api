using HanimeliApp.Admin.Api.Controllers.BaseControllers;
using HanimeliApp.Application.Models;
using HanimeliApp.Application.Services;
using HanimeliApp.Domain.Dtos.Address;
using HanimeliApp.Domain.Entities;
using HanimeliApp.Domain.Models.Address;
using Microsoft.AspNetCore.Mvc;

namespace HanimeliApp.Admin.Api.Controllers;

public class AddressController : CrudBaseController<AddressService, Address, AddressModel, CreateAddressRequest,
    UpdateAddressRequest>
{
    public AddressController(AddressService service) : base(service)
    {
    }

    [HttpGet]
    public async Task<Result<List<AddressModel>>> GetListByUserId([FromQuery] int userId, [FromQuery] int pageNumber)
    {
        var models = await Service.GetListByUser(userId, pageNumber, 25);
        return Result.AsSuccess(models);
    }
}