using System.Security.Claims;
using HanimeliApp.Admin.Api.Controllers.BaseControllers;
using HanimeliApp.Application.Models;
using HanimeliApp.Application.Services;
using HanimeliApp.Domain.Dtos.Order;
using HanimeliApp.Domain.Models.Order;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HanimeliApp.Admin.Api.Controllers;

public class OrderController : AdminBaseController
{
    private readonly OrderService _orderService;

    public OrderController(OrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpGet]
    [Authorize(Policy = "B2BPolicy")]
    public async Task<Result<List<OrderModel>>> GetList([FromQuery] int pageNumber)
    {
        var isAdmin = User.IsInRole("Admin");
        var filterModel = new OrderFilterModel();
        if (!isAdmin)
        {
            filterModel.UserId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
        }
        var models = await _orderService.GetOrdersAsync(null, pageNumber, 25);
        return Result.AsSuccess(models);
    }

    [HttpPost]
    [Authorize(Policy = "B2BPolicy")]
    public async Task<Result<List<OrderModel>>> CreateOrders([FromBody] CreateB2BOrderRequest request)
    {
        var isAdmin = User.IsInRole("Admin");
        if (!isAdmin)
        {
            request.Orders.ForEach(x => x.UserId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier)));
        }
        var orderModel = await _orderService.CreateB2BOrders(request);
        return Result.AsSuccess(orderModel);
    }
}