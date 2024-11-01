using Azure.Core;
using HanimeliApp.Admin.Api.Controllers.BaseControllers;
using HanimeliApp.Application.Models;
using HanimeliApp.Application.Services;
using HanimeliApp.Domain.Dtos.Address;
using HanimeliApp.Domain.Entities;
using HanimeliApp.Domain.Models.Address;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HanimeliApp.Admin.Api.Controllers;

public class AddressController : CrudBaseController<AddressService, Address, AddressModel, CreateAddressRequest,
    UpdateAddressRequest>
{
    public AddressController(AddressService service) : base(service)
    {
    }

    [HttpPost]
    public override Task<Result<AddressModel>> Create([FromBody] CreateAddressRequest request)
    {
        var isAdmin = User.IsInRole("Admin");
        if (!isAdmin)
        {
            request.UserId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
        }
        return base.Create(request);
    }

    [HttpPost]
    public override Task<Result<AddressModel>> Update(int id, [FromBody] UpdateAddressRequest request)
    {
        var isAdmin = User.IsInRole("Admin");
        if (!isAdmin)
        {
            request.UserId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
        }
        return base.Update(id, request);
    }

    [HttpGet]
    public async Task<Result<List<AddressModel>>> GetListByUserId([FromQuery] int userId, [FromQuery] int pageNumber)
    {
        var isAdmin = User.IsInRole("Admin");
        if (!isAdmin)
        {
            userId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
        }
        var models = await Service.GetListByUser(userId, pageNumber, 25);
        return Result.AsSuccess(models);
    }
}