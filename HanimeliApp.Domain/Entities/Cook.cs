using HanimeliApp.Domain.Entities.Abstract;

namespace HanimeliApp.Domain.Entities;

public class Cook : BaseEntity<int>
{
    public string Name { get; set; }
    public string? Bio { get; set; }
    public decimal Rating { get; set; }
    public string? ImageUrl { get; set; }
    public string? Iban { get; set; }
    public string? Address { get; set; }

    // Navigation Properties
    public virtual ICollection<Menu> Menus { get; set; }
    public virtual ICollection<Rating> Ratings { get; set; }
}
