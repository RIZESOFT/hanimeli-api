using System.Linq.Expressions;
using AutoMapper;
using HanimeliApp.Application.Exceptions;
using HanimeliApp.Application.Services.Abstract;
using HanimeliApp.Application.Utilities;
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

    public async Task<List<OrderModel>> GetOrdersAsync(OrderFilterModel? filterModel, int pageNumber, int pageSize)
    {
        Expression<Func<Order, bool>> filter = x => true;
        if (filterModel != null)
        {
            if (filterModel.CookId.HasValue)
            {
                filter = filter.AndAlso(x => x.OrderItems.Any(y => y.CookId == filterModel.CookId));
            }

            if (filterModel.UserId.HasValue)
            {
                filter = filter.AndAlso(x => x.UserId == filterModel.UserId);
            }
            
            if (filterModel.CourierId.HasValue)
            {
                filter = filter.AndAlso(x => x.CourierId == filterModel.CourierId);
            }
        }
        
        var paging = new EntityPaging();
        paging.PageNumber = pageNumber;
        paging.ItemCount = pageSize;
        var entities = await UnitOfWork.Repository<Order>().GetListAsync(filter, x => x.OrderBy(y => y.Id), paging: paging);
        var models = Mapper.Map<List<OrderModel>>(entities);
        return models;
    }
    
    public async Task<List<OrderModel>> CreateB2BOrders(CreateB2BOrderRequest request)
    {
        var orders = new List<OrderModel>();
        foreach (var orderRequest in request.Orders)
        {
            orders.Add(await CreateB2BOrder(orderRequest));
        }
        return orders;
    }
    
    public async Task<OrderModel> CreateB2BOrder(B2BOrderRequest request)
    {
        var user = await UnitOfWork.Repository<User>().GetAsync(x => x.Id == request.UserId);
        if (user is null)
            throw ValidationExceptions.InvalidUser;
        
        var address = await UnitOfWork.Repository<Address>().GetAsync(x => x.Id == request.AddressId && x.UserId == request.UserId);
        if (address is null)
            throw ValidationExceptions.InvalidAddress;
        
        if (request.OrderItems.Count == 0)
            throw ValidationExceptions.OrderItemsRequired;
        
        //if (request.OrderItems.Any(x => !x.CookId.HasValue))
        //    throw ValidationExceptions.OrderItemsCookRequired;
        
        if (request.OrderItems.Any(x => !x.MenuId.HasValue))
            throw ValidationExceptions.OrderItemsMenuRequired;
        
        
        var menuIds = request.OrderItems.Select(x => x.MenuId!.Value);
        var menus = await UnitOfWork.Repository<Menu>().GetListAsync(x => menuIds.Contains(x.Id));
        var order = Mapper.Map<Order>(request);
        order.Status = request.OrderItems.All(x => x.CookId.HasValue) ? OrderStatus.AssignedToCook : OrderStatus.Created; 
        order.OrderItems = request.OrderItems.Select(x => new OrderItem
        {
            MenuId = x.MenuId,
            CookId = x.CookId,
            Status = x.CookId.HasValue ? OrderItemStatus.AssignedToCook : OrderItemStatus.Created,
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