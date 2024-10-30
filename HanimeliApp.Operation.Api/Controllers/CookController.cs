using System.Security.Claims;
using HanimeliApp.Application.Models;
using HanimeliApp.Application.Services;
using HanimeliApp.Domain.Dtos.Cook;
using HanimeliApp.Domain.Models.Cook;
using HanimeliApp.Domain.Models.Order;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;

namespace HanimeliApp.Operation.Api.Controllers;

[Authorize(Policy = "CookPolicy")]
[Route("api/{culture}/[controller]/[action]")]
public class CookController : ControllerBase
{
    private readonly OrderService _orderService;
    private readonly CookService _cookService;

    public CookController(OrderService orderService, CookService cookService)
    {
        _orderService = orderService;
        _cookService = cookService;
    }

    [HttpGet] 
    public async Task<Result<CookDashboardModel>> GetDashboard()
    {
        var userId = Convert.ToInt32(User.FindFirstValue(JwtRegisteredClaimNames.Sub));
        var model = await _cookService.GetDashboard(userId);
        return Result.AsSuccess(model);
    }
    
    [HttpPost] 
    public async Task<Result> UpdateOrderStatus([FromBody] UpdateOrderStatusRequest request)
    {
        var userId = Convert.ToInt32(User.FindFirstValue(JwtRegisteredClaimNames.Sub));
        await _cookService.UpdateOrderStatus(userId, request);
        return Result.AsSuccess();
    }
    
    [HttpGet]
    public async Task<Result<List<OrderModel>>> GetOrders([FromQuery] DateTime startDate, [FromQuery] DateTime endDate, [FromQuery] int pageNumber)
    {
        var userId = Convert.ToInt32(User.FindFirstValue(JwtRegisteredClaimNames.Sub));
        var models = await _orderService.GetOrdersAsync(new OrderFilterModel { DeliveryDateStart = startDate, DeliveryDateEnd = endDate, CookId = userId }, pageNumber, 25);
        return Result.AsSuccess(models);
    }
}