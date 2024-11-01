using AutoMapper;
using HanimeliApp.Application.Exceptions;
using HanimeliApp.Application.Services.Abstract;
using HanimeliApp.Domain.Dtos.Beverage;
using HanimeliApp.Domain.Dtos.Cook;
using HanimeliApp.Domain.Dtos.Food;
using HanimeliApp.Domain.Entities;
using HanimeliApp.Domain.Enums;
using HanimeliApp.Domain.Models.Beverage;
using HanimeliApp.Domain.Models.Cook;
using HanimeliApp.Domain.Models.Food;
using HanimeliApp.Domain.Models.Order;
using HanimeliApp.Domain.UnitOfWorks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HanimeliApp.Application.Services;

public class CookService : ServiceBase<Cook, CookModel, CreateCookRequest, UpdateCookRequest>
{
    private readonly ImageService _imageService;
    public CookService(IUnitOfWork unitOfWork, IMapper mapper, ImageService imageService) : base(unitOfWork, mapper)
    {
        _imageService = imageService;
    }

    public override async Task<CookModel> Create(CreateCookRequest request)
    {
        var userRepository = UnitOfWork.Repository<User>();
        var user = await userRepository.GetAsync(x => x.Email == request.Email || x.Phone == request.Phone);

        if (user != null)
            throw ValidationExceptions.UserAlreadyExists;
            
        var hasher = new PasswordHasher<object>();
        var dummyUser = new object();
        var hashedPassword = hasher.HashPassword(dummyUser, request.Password);

        user = Mapper.Map<User>(request);
        user.Password = hashedPassword;
        user.Cook = Mapper.Map<Cook>(request);

            
        await userRepository.InsertAsync(user);
        await UnitOfWork.SaveChangesAsync();
        await UnitOfWork.SaveChangesAsync();
        var model = Mapper.Map<CookModel>(user.Cook);
        return model;
    }

    public async Task<CookModel> CreateWithImage(CreateCookRequest request, IFormFile imageFile)
    {
        var imageUrl = await _imageService.UploadImageAsync(imageFile, "images");
        var userRepository = UnitOfWork.Repository<User>();
        var user = await userRepository.GetAsync(x => x.Email == request.Email || x.Phone == request.Phone);

        if (user != null)
            throw ValidationExceptions.UserAlreadyExists;

        var hasher = new PasswordHasher<object>();
        var dummyUser = new object();
        var hashedPassword = hasher.HashPassword(dummyUser, request.Password);

        user = Mapper.Map<User>(request);
        user.Password = hashedPassword;
        user.Cook = Mapper.Map<Cook>(request);


        await userRepository.InsertAsync(user);
        await UnitOfWork.SaveChangesAsync();
        await UnitOfWork.SaveChangesAsync();
        var model = Mapper.Map<CookModel>(user.Cook);
        return model;
    }

    public async Task<CookModel> UpdateWithImage(int id, UpdateCookRequest request, IFormFile imageFile)
    {
        var entity = await UnitOfWork.Repository<Cook>().GetAsync(x => x.Id == id);

        if (entity == null)
            throw ValidationExceptions.RecordNotFound;

        Mapper.Map(request, entity);

        if (imageFile != null)
        {
            var imageUrl = await _imageService.UploadImageAsync(imageFile, "images");
            entity.ImageUrl = imageUrl;
        }
        
        UnitOfWork.Repository<Cook>().Update(entity);
        await UnitOfWork.SaveChangesAsync();
        var model = Mapper.Map<CookModel>(entity);
        return model;
    }
    public async Task<CookDashboardModel> GetDashboard(int userId)
    {
        const int profitPerMenu = 30;
        var user = await UnitOfWork.Repository<User>().GetAsync(x => x.Id == userId);
        var model = new CookDashboardModel();
        var userOrders = await UnitOfWork.Repository<Order>().GetListAsync(order => 
                                                    order.OrderItems.Any(y => y.CookId == user!.CookId) 
                                                    && order.DeliveryDate > DateTime.Now,
                                            orders => orders.OrderBy(x => x.DeliveryDate),
                                            order => 
                                                order.Include(y => y.OrderItems).ThenInclude(y => y.Menu).ThenInclude(y => y.Foods)
                                                .Include(y => y.OrderItems).ThenInclude(y => y.Menu).ThenInclude(y => y.Beverages)
                                            );
        var completedMenus = await UnitOfWork.Repository<OrderItem>().CountAsync(x => x.Order.DeliveryDate.Value.Date == DateTime.Now.Date && x.Status == OrderItemStatus.Completed);
        if (userOrders.Count != 0)
        {
            model.NextOrder = Mapper.Map<OrderModel>(userOrders.FirstOrDefault());
            if (model.NextOrder != null)
            {
                model.NextOrder.OrderItems = model.NextOrder.OrderItems.Where(x => x.CookId == user.CookId).ToList();
            }

            model.DailyOrders = Mapper.Map<List<OrderModel>>(userOrders.Where(x => x.DeliveryDate!.Value.Date == DateTime.Now.Date));
            model.DailyOrders.ForEach(x => x.OrderItems = x.OrderItems.Where(y => y.CookId == user.CookId).ToList());
        }
        model.BalanceAmount = completedMenus * profitPerMenu;
        model.MonthlyTotalOrderCount = completedMenus;
        return model;
    }

    public async Task UpdateOrderStatus(int userId, UpdateOrderStatusRequest request)
    {
        var user = await UnitOfWork.Repository<User>().GetAsync(x => x.Id == userId);
        var order = await UnitOfWork.Repository<Order>().GetAsync(x => x.Id == request.OrderId,
            includes: order => order.Include(y => y.OrderItems));
        
        if (order == null)
            throw new NullReferenceException($"Order with id: {request.OrderId} not found");

        if (order.Status == OrderStatus.DeliveredToCourier)
            throw new Exception($"Order with id: {request.OrderId} already delivered to courier");

        var orderItems = order.OrderItems.Where(x => request.OrderItemIds.Contains(x.Id)).ToList();
        if (orderItems.Any(x => x.Status == OrderItemStatus.Completed))
            throw new Exception($"Order with id: {request.OrderId} already delivered to courier");

        if (orderItems.Any(x => x.CookId != user.CookId))
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