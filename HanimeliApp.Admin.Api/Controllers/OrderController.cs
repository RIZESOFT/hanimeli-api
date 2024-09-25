using HanimeliApp.Application.Models;
using HanimeliApp.Application.Services;
using HanimeliApp.Domain.Models.Order;
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
    public async Task<Result<List<OrderModel>>> GetList([FromQuery] int pageNumber)
    {
        var models = await _orderService.GetAllOrdersAsync(pageNumber, 25);
        return Result.AsSuccess(models);
    }
}