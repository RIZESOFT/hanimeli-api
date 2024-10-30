namespace HanimeliApp.Domain.Dtos.Cook;

public class UpdateOrderStatusRequest
{
    public int OrderId { get; set; }
    public List<int> OrderItemIds { get; set; }
}