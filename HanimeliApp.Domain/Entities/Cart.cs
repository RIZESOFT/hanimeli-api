using HanimeliApp.Domain.Entities.Abstract;

namespace HanimeliApp.Domain.Entities;

public class Cart : BaseEntity<int>
{
    public int UserId { get; set; }
    public DateTime CreatedDate { get; set; }
    
    public ICollection<CartItem> CartItems { get; set; }
}
