using HanimeliApp.Domain.Entities.Abstract;

namespace HanimeliApp.Domain.Entities;

public class Favorite : BaseEntity<int>
{
    public int UserId { get; set; }
    public int MenuId { get; set; }
    public int CookId { get; set; }

    // Navigation Properties
    public User User { get; set; }
    public Menu Menu { get; set; }
    public Cook Cook { get; set; }
}