using HanimeliApp.Domain.Entities;
using HanimeliApp.Domain.Enums;
using HanimeliApp.Domain.Models.Address;
using HanimeliApp.Domain.Models.Cook;
using HanimeliApp.Domain.Models.User;

namespace HanimeliApp.Domain.Models.Order;

public class OrderModel
{
    public int UserId { get; set; }
    public int AddressId { get; set; }
    public int? CookId { get; set; }
    public decimal TotalAmount { get; set; }
    public OrderStatus Status { get; set; }
    public DateTime OrderDate { get; set; }
    public DateTime? DeliveryDate { get; set; }
    public DateTime? ActualDeliveryDate { get; set; }
    

    public AddressModel DeliveryAddress { get; set; } = null!;
    public UserModel User { get; set; } = null!;
    public CookModel? Cook { get; set; }
    public List<OrderItemModel> OrderItems { get; set; } = null!;
}