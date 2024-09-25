using AutoMapper;
using HanimeliApp.Domain.Dtos.Cook;
using HanimeliApp.Domain.Dtos.Order;
using HanimeliApp.Domain.Entities;
using HanimeliApp.Domain.Models.Cook;
using HanimeliApp.Domain.Models.Order;

namespace HanimeliApp.Application.MapperProfiles;

public class OrderProfile : Profile
{
    public OrderProfile()
    {
        CreateMap<Order, OrderModel>();
        CreateMap<OrderItem, OrderItemModel>();
        CreateMap<CreateB2BOrderRequest, Order>().ForMember(x => x.OrderItems, opt => opt.Ignore());
    }
}