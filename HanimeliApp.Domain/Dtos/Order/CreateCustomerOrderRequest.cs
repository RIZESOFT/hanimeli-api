namespace HanimeliApp.Domain.Dtos.Order;

public class CreateCustomerOrderRequest
{
    public int AddressId { get; set; }
    public DateTime? DeliveryDate { get; set; }
}