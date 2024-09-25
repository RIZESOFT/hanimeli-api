using AutoMapper;
using HanimeliApp.Application.Exceptions;
using HanimeliApp.Application.Services.Abstract;
using HanimeliApp.Domain.Dtos.Order;
using HanimeliApp.Domain.Entities;
using HanimeliApp.Domain.Enums;
using HanimeliApp.Domain.Models;
using HanimeliApp.Domain.Models.Order;
using HanimeliApp.Domain.UnitOfWorks;
using Microsoft.EntityFrameworkCore;

namespace HanimeliApp.Application.Services;

public class OrderService : ServiceBase
{
    public OrderService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
    {
    }

    public async Task<List<OrderModel>> GetAllOrdersAsync(int pageNumber, int pageSize)
    {
        var paging = new EntityPaging();
        paging.PageNumber = pageNumber;
        paging.ItemCount = pageSize;
        var entities = await UnitOfWork.Repository<Order>().GetListAsync(x => true, x => x.OrderBy(y => y.Id), paging: paging);
        var models = Mapper.Map<List<OrderModel>>(entities);
        return models;
    }
    
    public async Task<List<OrderModel>> GetOrdersByUserAsync(int userId, int pageNumber, int pageSize)
    {
        var paging = new EntityPaging();
        paging.PageNumber = pageNumber;
        paging.ItemCount = pageSize;
        var entities = await UnitOfWork.Repository<Order>().GetListAsync(x => x.UserId == userId, x => x.OrderBy(y => y.Id), paging: paging);
        var models = Mapper.Map<List<OrderModel>>(entities);
        return models;
    }
    
    public async Task<OrderModel> CreateB2BOrder(CreateB2BOrderRequest request)
    {
        var user = await UnitOfWork.Repository<User>().GetAsync(x => x.Id == request.UserId);
        if (user is null)
            throw ValidationExceptions.InvalidUser;
        
        if (request.OrderItems.Count == 0)
            throw ValidationExceptions.OrderItemsRequired;
        
        if (request.OrderItems.Any(x => !x.CookId.HasValue))
            throw ValidationExceptions.OrderItemsCookRequired;
        
        if (request.OrderItems.Any(x => !x.MenuId.HasValue))
            throw ValidationExceptions.OrderItemsMenuRequired;
        
        
        var menuIds = request.OrderItems.Select(x => x.MenuId.Value);
        var menus = await UnitOfWork.Repository<Menu>().GetListAsync(x => menuIds.Contains(x.Id));
        var order = Mapper.Map<Order>(request);
        order.Status = OrderStatus.AssignedToCook;
        order.OrderItems = request.OrderItems.Select(x => new OrderItem
        {
            MenuId = x.MenuId,
            CookId = x.CookId,
            Status = OrderItemStatus.AssignedToCook,
            Amount = menus.First(y => y.Id == x.MenuId).Price,
        }).ToList();
        order.TotalAmount = order.OrderItems.Sum(x => x.Amount);
        await UnitOfWork.Repository<Order>().InsertAsync(order);
        await UnitOfWork.SaveChangesAsync();
        var model = Mapper.Map<OrderModel>(order);
        return model;
    }
    
    public async Task<OrderModel> CreateCustomerOrder(int userId, CreateCustomerOrderRequest request)
    {
        var user = await UnitOfWork.Repository<User>().GetAsync(x => x.Id == userId);
        if (user is null)
            throw ValidationExceptions.InvalidUser;
        
        var cart = await UnitOfWork.Repository<Cart>().GetAsync(x => x.UserId == userId, x => x.Include(y => y.CartItems));
        if (cart is null || cart.CartItems.Count == 0)
            throw ValidationExceptions.CartNotFound;
        
        var entity = Mapper.Map<Order>(request);
        
        var model = Mapper.Map<OrderModel>(entity);
        return model;
    }
}