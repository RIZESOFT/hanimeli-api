namespace HanimeliApp.Domain.Models.Order;

public class OrderFilterModel
{
    public int? UserId { get; set; }
    public int? CookId { get; set; }
    public int? CourierId { get; set; }
    public DateTime? OrderDateStart { get; set; }
    public DateTime? OrderDateEnd { get; set; }
    public DateTime? DeliveryDateStart { get; set; }
    public DateTime? DeliveryDateEnd { get; set; }
    public int? CookUserId { get; set; }
    public int? CourierUserId { get; set; }
}