using HanimeliApp.Domain.Entities.Abstract;

namespace HanimeliApp.Domain.Entities;

public class CartItem : BaseEntity<int>
{
    public int CartId { get; set; }
    public int? MenuId { get; set; }
    public int? FoodId { get; set; }
    public int? BeverageId { get; set; }

    public virtual Cart Cart { get; set; } = null!;
    public virtual Menu? Menu { get; set; }
    public virtual Food? Food { get; set; }
    public virtual Beverage? Beverage { get; set; }
}