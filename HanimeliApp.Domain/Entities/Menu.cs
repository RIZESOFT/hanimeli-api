using HanimeliApp.Domain.Entities.Abstract;

namespace HanimeliApp.Domain.Entities;

public class Menu : BaseEntity<int>
{
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }

    // Navigation Properties
    public ICollection<Food> Foods { get; set; }
    public ICollection<Beverage> Beverages { get; set; }
    public ICollection<Cook> Cooks { get; set; }
    public ICollection<Rating> Ratings { get; set; }
}
