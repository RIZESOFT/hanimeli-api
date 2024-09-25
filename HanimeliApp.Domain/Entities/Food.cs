using HanimeliApp.Domain.Entities.Abstract;

namespace HanimeliApp.Domain.Entities;

public class Food : BaseEntity<int>
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public decimal Price { get; set; }
    public string? ImageUrl { get; set; }
    public bool IsActive { get; set; }
    
    // Navigation Properties
    public ICollection<Menu> Menus { get; set; }
}