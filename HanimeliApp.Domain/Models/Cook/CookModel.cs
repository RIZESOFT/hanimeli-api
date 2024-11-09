using HanimeliApp.Domain.Models.Order;
using HanimeliApp.Domain.Models.User;

namespace HanimeliApp.Domain.Models.Cook;

public class CookModel
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Name { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string Nickname { get; set; }
    public string? Bio { get; set; }
    public decimal Rating { get; set; }
    public string? Iban { get; set; }
    public string? ImageUrl { get; set; }
    public string? Address { get; set; }
    
    public List<OrderItemModel>? AssignedActiveOrderItems { get; set; }
}