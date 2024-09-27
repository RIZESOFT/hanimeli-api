using HanimeliApp.Domain.Entities.Abstract;
using HanimeliApp.Domain.Enums;

namespace HanimeliApp.Domain.Entities;

public class Order : BaseEntity<int>
{
    public int UserId { get; set; }
    public int AddressId { get; set; }
    public decimal TotalAmount { get; set; }
    public OrderStatus Status { get; set; }
    public DateTime OrderDate { get; set; }
    public DateTime? DeliveryDate { get; set; }
    public DateTime? ActualDeliveryDate { get; set; }
    public int? CourierId { get; set; }
    

    // Navigation Properties
    public virtual Address DeliveryAddress { get; set; } = null!;
    public virtual User User { get; set; } = null!;
    public virtual Courier? Courier { get; set; }
    public virtual ICollection<OrderItem> OrderItems { get; set; } = null!;
}
