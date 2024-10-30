using AutoMapper;
using HanimeliApp.Application.Services.Abstract;
using HanimeliApp.Domain.Dtos.Beverage;
using HanimeliApp.Domain.Dtos.Cook;
using HanimeliApp.Domain.Entities;
using HanimeliApp.Domain.Enums;
using HanimeliApp.Domain.Models.Beverage;
using HanimeliApp.Domain.Models.Cook;
using HanimeliApp.Domain.Models.Order;
using HanimeliApp.Domain.UnitOfWorks;
using Microsoft.EntityFrameworkCore;

namespace HanimeliApp.Application.Services;

public class CookService : ServiceBase<Cook, CookModel, CreateCookRequest, UpdateCookRequest>
{
    public CookService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
    {
    }

    public async Task<CookDashboardModel> GetDashboard(int userId)
    {
        const int profitPerMenu = 30;
        var model = new CookDashboardModel();
        var userOrders = await UnitOfWork.Repository<Order>().GetListAsync(order => 
                                                    order.OrderItems.Any(y => y.CookId == userId) 
                                                    && order.DeliveryDate > DateTime.Now,
                                            orders => orders.OrderBy(x => x.DeliveryDate),
                                            order => order.Include(y => y.OrderItems));
        var completedMenus = await UnitOfWork.Repository<OrderItem>().CountAsync(x => x.Order.DeliveryDate.Value.Date == DateTime.Now.Date && x.Status == OrderItemStatus.Completed);
        if (userOrders.Count != 0)
        {
            model.NextOrder = Mapper.Map<OrderModel>(userOrders.FirstOrDefault());
            if (model.NextOrder != null)
            {
                model.NextOrder.OrderItems = model.NextOrder.OrderItems.Where(x => x.CookId == userId).ToList();
            }

            model.DailyOrders = Mapper.Map<List<OrderModel>>(userOrders.Where(x => x.DeliveryDate!.Value.Date == DateTime.Now.Date));
            model.DailyOrders.ForEach(x => x.OrderItems = x.OrderItems.Where(y => y.CookId == userId).ToList());
        }
        model.BalanceAmount = completedMenus * profitPerMenu;
        model.MonthlyTotalOrderCount = completedMenus;
        return model;
    }

    public async Task UpdateOrderStatus(int userId, UpdateOrderStatusRequest request)
    {
        var order = await UnitOfWork.Repository<Order>().GetAsync(x => x.Id == request.OrderId,
            includes: order => order.Include(y => y.OrderItems));
        
        if (order == null)
            throw new NullReferenceException($"Order with id: {request.OrderId} not found");

        if (order.Status == OrderStatus.DeliveredToCourier)
            throw new Exception($"Order with id: {request.OrderId} already delivered to courier");

        var orderItems = order.OrderItems.Where(x => request.OrderItemIds.Contains(x.Id)).ToList();
        if (orderItems.Any(x => x.Status == OrderItemStatus.Completed))
            throw new Exception($"Order with id: {request.OrderId} already delivered to courier");

        if (orderItems.Any(x => x.CookId != userId))
            throw new Exception($"Order with id: {request.OrderId} does not belong to user");
            
        foreach (var orderItem in orderItems)
        {
            orderItem.Status = OrderItemStatus.Completed;
            UnitOfWork.Repository<OrderItem>().Update(orderItem);
        }
        
        if (order.OrderItems.All(x => x.Status == OrderItemStatus.Completed))
        {
            order.Status = OrderStatus.AssignedToCook;
            UnitOfWork.Repository<Order>().Update(order);
        }
        await UnitOfWork.SaveChangesAsync();
    }
}