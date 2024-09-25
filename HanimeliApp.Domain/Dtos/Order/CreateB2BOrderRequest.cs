using HanimeliApp.Domain.Models.Order;

namespace HanimeliApp.Domain.Dtos.Order;

public class CreateB2BOrderRequest
{
    public int UserId { get; set; }
    public int AddressId { get; set; }
    public DateTime? DeliveryDate { get; set; }
    public List<B2BOrderItemRequest> OrderItems { get; set; }
}

public class B2BOrderItemRequest
{
    public int? MenuId { get; set; }
    public int? FoodId { get; set; }
    public int? BeverageId { get; set; }
    public int? CookId { get; set; }
    public decimal Amount { get; set; }
}