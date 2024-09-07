namespace HanimeliApp.Domain.Dtos.Menu;

public class UpdateMenuRequest
{
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    
    public List<int>? FoodIds { get; set; }
    public List<int>? BeverageIds { get; set; }
}