using HanimeliApp.Domain.Entities;
using HanimeliApp.Domain.Models.Beverage;
using HanimeliApp.Domain.Models.Food;

namespace HanimeliApp.Domain.Models.Menu;

public class MenuModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public string ImageUrl { get; set; }

    public List<FoodModel> Foods { get; set; }
    public List<BeverageModel> Beverages { get; set; }
}