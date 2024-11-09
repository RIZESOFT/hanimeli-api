using AutoMapper;
using HanimeliApp.Application.Utilities;
using HanimeliApp.Domain.Dtos.Cook;
using HanimeliApp.Domain.Dtos.Order;
using HanimeliApp.Domain.Entities;
using HanimeliApp.Domain.Models.Beverage;
using HanimeliApp.Domain.Models.Cook;
using HanimeliApp.Domain.Models.Food;
using HanimeliApp.Domain.Models.Menu;
using HanimeliApp.Domain.Models.Order;

namespace HanimeliApp.Application.MapperProfiles;

public class OrderProfile : Profile
{
    public OrderProfile()
    {
        CreateMap<Order, OrderModel>()
            .ForMember(o => o.OrderItems, s => s.MapFrom((x, _) => x.OrderItems.GroupOrderItemsForUi()));
        CreateMap<B2BOrderRequest, Order>().ForMember(x => x.OrderItems, opt => opt.Ignore());
    }
}