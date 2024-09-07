namespace HanimeliApp.Domain.Dtos.Beverage;

public class CreateBeverageRequest
{
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
}