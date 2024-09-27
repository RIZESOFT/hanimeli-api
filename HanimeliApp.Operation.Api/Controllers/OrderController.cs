using System.Security.Claims;
using HanimeliApp.Application.Models;
using HanimeliApp.Application.Services;
using HanimeliApp.Domain.Models.Order;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;

namespace HanimeliApp.Operation.Api.Controllers;

[Route("api/{culture}/[controller]/[action]")]
public class OrderController : ControllerBase
{
    private readonly OrderService _orderService;

    public OrderController(OrderService orderService)
    {
        _orderService = orderService;
    }

    [Authorize(Policy = "CookPolicy")]
    [HttpGet]
    public async Task<Result<List<OrderModel>>> GetOrdersForCook([FromQuery] int pageNumber)
    {
        var userId = Convert.ToInt32(User.FindFirstValue(JwtRegisteredClaimNames.Sub));
        var models = await _orderService.GetOrdersAsync(new OrderFilterModel { CookId = userId }, pageNumber, 25);
        return Result.AsSuccess(models);
    }

    [Authorize(Policy = "CourierPolicy")]
    [HttpGet]
    public async Task<Result<List<OrderModel>>> GetOrdersForCourier([FromQuery] int pageNumber)
    {
        var userId = Convert.ToInt32(User.FindFirstValue(JwtRegisteredClaimNames.Sub));
        var models = await _orderService.GetOrdersAsync(new OrderFilterModel { CourierId = userId }, pageNumber, 25);
        return Result.AsSuccess(models);
    }
}