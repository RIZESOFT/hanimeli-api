using System.Security.Claims;
using HanimeliApp.Admin.Api.Controllers.BaseControllers;
using HanimeliApp.Application.Models;
using HanimeliApp.Application.Services;
using HanimeliApp.Domain.Dtos.Order;
using HanimeliApp.Domain.Models.Order;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HanimeliApp.Admin.Api.Controllers;

public class OrderController : BaseController
{
    private readonly OrderService _orderService;

    public OrderController(OrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpGet]
    public async Task<Result<List<OrderModel>>> GetList([FromQuery] GetOrderListRequest request)
    {
        var isAdmin = User.IsInRole("Admin");
        var filterModel = new OrderFilterModel
        {
            OrderDateStart = request.OrderDateStart,
            OrderDateEnd = request.OrderDateEnd,
            DeliveryDateStart = request.DeliveryDateStart,
            DeliveryDateEnd = request.DeliveryDateEnd,
        };
        if (!isAdmin)
        {
            filterModel.UserId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
        }
        var models = await _orderService.GetOrdersAsync(filterModel, request.PageNumber, 25);
        return Result.AsSuccess(models);
    }

    [HttpPost]
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
    
    [Authorize(Policy = "AdminPolicy")]
    [HttpGet]
    public async Task<Result<List<AssigmentOrderModel>>> GetB2BListForAssigment([FromQuery] DateTime startDate,[FromQuery] DateTime endDate)
    {
        var models = await _orderService.GetB2BListForAssigment(startDate, endDate);
        return Result.AsSuccess(models);
    }

    [Authorize(Policy = "AdminPolicy")]
    [HttpPost]
    public async Task<Result> AssignB2BOrders([FromBody] AssignB2BOrdersRequest request)
    {
        await _orderService.AssignB2BOrders(request);
        return Result.AsSuccess("Ok");
    }
    
    [Authorize(Policy = "AdminPolicy")]
    [HttpPut("{cookId:int}")]
    public virtual async Task<Result> UnassignB2BOrders(int cookId)
    {
        await _orderService.UnassignB2BOrders(cookId);
        return Result.AsSuccess("Ok");
    }
}