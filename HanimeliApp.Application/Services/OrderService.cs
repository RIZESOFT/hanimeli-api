using System.Globalization;
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
            if (filterModel.DeliveryDateStart.HasValue)
            {
                filter = filter.AndAlso(x => x.DeliveryDate >= filterModel.DeliveryDateStart.Value);
            }
            
            if (filterModel.DeliveryDateEnd.HasValue)
            {
                filter = filter.AndAlso(x => x.DeliveryDate <= filterModel.DeliveryDateEnd.Value);
            }

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
        var entities = await UnitOfWork.Repository<Order>().GetListAsync(filter, x => x.OrderBy(y => y.Id), x => x.Include(y => y.OrderItems), paging: paging);
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
        order.OrderDate = DateTime.Now;
        order.Status = request.OrderItems.All(x => x.CookId.HasValue) ? OrderStatus.AssignedToCook : OrderStatus.Created;
        order.OrderItems = new List<OrderItem>();
        foreach (var orderItem in request.OrderItems)
        {
            for (var i = 0; i < orderItem.Quantity; i++)
            {
                order.OrderItems.Add(new OrderItem()
                {
                    MenuId = orderItem.MenuId,
                    CookId = orderItem.CookId,
                    Status = orderItem.CookId.HasValue ? OrderItemStatus.AssignedToCook : OrderItemStatus.Created,
                    Amount = orderItem.Amount ?? menus.First(y => y.Id == orderItem.MenuId).Price,
                });
            }
        }
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

    public async Task<List<AssigmentOrderModel>> GetB2BListForAssigment(DateTime startDate, DateTime endDate)
    {
        Expression<Func<Order, bool>> filter = x => x.DeliveryDate >= startDate 
                                                    && x.DeliveryDate <= endDate
                                                    && x.User.Role == Roles.B2B
                                                    && x.OrderItems.Any(y => !y.CookId.HasValue);
        var orders = await UnitOfWork.Repository<Order>().GetListAsync(filter, x => x.OrderBy(y => y.UserId),
            includes: x => x.Include(y => y.OrderItems));

        var list = new List<AssigmentOrderModel>();
        var groupedOrders = orders.GroupBy(x => x.DeliveryDate!.Value.Date);
        foreach (var groupedOrder in groupedOrders)
        {
            var hourlyMenus = groupedOrder.GroupBy(x => $"{x.DeliveryDate!.Value.Hour}:{x.DeliveryDate.Value.Minute}").Select(x =>
            {
                return new
                {
                    Hour = x.Key,
                    MenuIds = x.SelectMany(y => y.OrderItems.Where(z => !z.CookId.HasValue).Select(z => z.MenuId!.Value)).ToList()
                };
            }).ToList();
            var item = new AssigmentOrderModel
            {
                DayName = groupedOrder.Key.ToString("dddd", CultureInfo.CurrentUICulture),
                Date = groupedOrder.Key,
                Hours = hourlyMenus.Select(x => new AssigmentOrderHourModel
                {
                    Hour = x.Hour,
                    Menus = x.MenuIds.GroupBy(y => y).Select(y => new AssigmentOrderHourMenuModel()
                    {
                        MenuId = y.Key,
                        Count = y.Count(),
                    }).ToList()
                }).ToList()
            };
            list.Add(item);
        }
        return list;
    }

    public async Task AssignB2BOrders(AssignB2BOrdersRequest request)
    {
        var hours = request.Hour.Split(":").Select(x => Convert.ToInt32(x)).ToList();
        var date = request.Date.Date;
        date = date.AddHours(hours[0]);
        date = date.AddMinutes(hours[1]);
        var endDate = date.AddMinutes(1);
        Expression<Func<Order, bool>> filter = x => x.DeliveryDate >= date 
                                                    && x.DeliveryDate <= endDate
                                                    && x.User.Role == Roles.B2B
                                                    && x.OrderItems.Any(y => !y.CookId.HasValue);
        var orders = await UnitOfWork.Repository<Order>().GetListAsync(filter, x => x.OrderBy(y => y.UserId),
            includes: x => x.Include(y => y.OrderItems));

        var orderItems = orders.SelectMany(x => x.OrderItems).Where(x => !x.CookId.HasValue && x.MenuId == request.MenuId).ToList();
        foreach (var orderItem in orderItems.Take(request.MenuCount))
        {
            orderItem.CookId = request.CookId;
            orderItem.Status = OrderItemStatus.AssignedToCook;
            UnitOfWork.Repository<OrderItem>().Update(orderItem);
        }
        
        foreach (var order in orders.Where(x => x.OrderItems.All(y => y.CookId.HasValue)))
        {
            order.Status = OrderStatus.AssignedToCook;
            UnitOfWork.Repository<Order>().Update(order);
        }
        
        await UnitOfWork.SaveChangesAsync();
    }
}