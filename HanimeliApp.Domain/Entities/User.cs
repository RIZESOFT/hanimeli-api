using HanimeliApp.Domain.Entities.Abstract;

namespace HanimeliApp.Domain.Entities;

public class User : BaseEntity<int>
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string Password { get; set; }

    // Navigation Properties
    public ICollection<Order> Orders { get; set; }
    public ICollection<Favorite> Favorites { get; set; }
    public ICollection<Address> Addresses { get; set; }
}
