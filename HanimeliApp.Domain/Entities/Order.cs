using HanimeliApp.Domain.Entities.Abstract;

namespace HanimeliApp.Domain.Entities;

public class Order : BaseEntity<int>
{
    public int UserId { get; set; }
    public int AddressId { get; set; }
    public ICollection<Menu> Menus { get; set; }
    public ICollection<Beverage> Beverages { get; set; }
    public decimal TotalAmount { get; set; }
    public string Status { get; set; }
    public DateTime OrderDate { get; set; }

    // Navigation Properties
    public Address DeliveryAddress { get; set; }
    public User User { get; set; }
}
