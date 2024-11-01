using HanimeliApp.Domain.Dtos.User;

namespace HanimeliApp.Domain.Dtos.Cook;

public class CreateCookRequest : CreateUserRequest
{
    public string Nickname { get; set; }
    public string? Bio { get; set; }
    public string? Iban { get; set; }
    public string? Address { get; set; }
}