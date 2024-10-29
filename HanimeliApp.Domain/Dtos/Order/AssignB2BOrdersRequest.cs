namespace HanimeliApp.Domain.Dtos.Order;

public class AssignB2BOrdersRequest
{
    public DateTime Date { get; set; }
    public string Hour { get; set; }
    public int MenuId { get; set; }
    public int MenuCount { get; set; }
    public int CookId { get; set; }
}