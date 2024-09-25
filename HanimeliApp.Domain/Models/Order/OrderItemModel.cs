using HanimeliApp.Domain.Models.Beverage;
using HanimeliApp.Domain.Models.Cook;
using HanimeliApp.Domain.Models.Food;
using HanimeliApp.Domain.Models.Menu;

namespace HanimeliApp.Domain.Models.Order;

public class OrderItemModel
{
    public int OrderId { get; set; }
    public int? MenuId { get; set; }
    public int? FoodId { get; set; }
    public int? BeverageId { get; set; }
    public int? CookId { get; set; }
    public decimal Amount { get; set; }
    
    public MenuModel Menu { get; set; }
    public FoodModel Food { get; set; }
    public BeverageModel Beverage { get; set; }
    public CookModel Cook { get; set; }
}