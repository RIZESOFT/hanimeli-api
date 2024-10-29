using HanimeliApp.Domain.Entities.Abstract;

namespace HanimeliApp.Domain.Entities;

public class User : BaseEntity<int>
{
    public string Name { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string Password { get; set; }
    public string Role { get; set; }
    public int? CookId { get; set; }
    public string? ProfilePictureUrl { get; set; }

    #region B2B
    public int? DailyOrderCount { get; set; }
    public int? AvailableWeekDays { get; set; }
    public string? OrderHours { get; set; }
    public string? OrderDays { get; set; }
    #endregion

    // Navigation Properties
    public ICollection<Order> Orders { get; set; }
    public ICollection<Favorite> Favorites { get; set; }
    public ICollection<Address> Addresses { get; set; }
    public Cook? Cook { get; set; }
}
