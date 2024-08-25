using HanimeliApp.Domain.Entities.Abstract;

namespace HanimeliApp.Domain.Entities;

public class CartItem : BaseEntity<int>
{
    public int CartId { get; set; }
    public int MenuId { get; set; }
    public int BeverageId { get; set; }
    public int Count { get; set; }

    public virtual Cart Cart { get; set; }
    public virtual Menu Menu { get; set; }
    public virtual Beverage Beverage { get; set; }
}