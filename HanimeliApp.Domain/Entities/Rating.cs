using HanimeliApp.Domain.Entities.Abstract;

namespace HanimeliApp.Domain.Entities;

public class Rating : BaseEntity<int>
{
    public int UserId { get; set; }
    public int MenuId { get; set; }
    public int CookId { get; set; }
    public decimal Score { get; set; }
    public string Comment { get; set; }

    // Navigation Properties
    public User User { get; set; }
    public Menu Menu { get; set; }
    public Cook Cook { get; set; }
}