namespace HanimeliApp.Domain.Models.Cook;

public class CookModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Bio { get; set; }
    public decimal Rating { get; set; }
    public string Iban { get; set; }
    public string ImageUrl { get; set; }
}