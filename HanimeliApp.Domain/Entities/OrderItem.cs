using HanimeliApp.Domain.Entities.Abstract;
using HanimeliApp.Domain.Enums;

namespace HanimeliApp.Domain.Entities;

public class OrderItem : BaseEntity<int>
{
    public int OrderId { get; set; }
    public int? MenuId { get; set; }
    public int? FoodId { get; set; }
    public int? BeverageId { get; set; }
    public int? CookId { get; set; }
    public OrderItemStatus Status { get; set; }
    public decimal Amount { get; set; }

    public virtual Order Order { get; set; } = null!;
    public virtual Menu? Menu { get; set; }
    public virtual Food? Food { get; set; }
    public virtual Beverage? Beverage { get; set; }
    public virtual Cook? Cook { get; set; }
}