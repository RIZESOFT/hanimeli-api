namespace HanimeliApp.Domain.Models.User;

public class OperationUserLoginResultModel : UserLoginResultModel
{
    public OperationUserLoginResultModel(int id, string firstname, string lastName, string email, string role, string token) : base(id, firstname, lastName, email, role, token)
    {
    }
    public OperationUserLoginResultModel(UserLoginResultModel userLoginResultModel)
        : base(userLoginResultModel.Id, userLoginResultModel.FirstName, userLoginResultModel.LastName, userLoginResultModel.Email, userLoginResultModel.Role, userLoginResultModel.AuthenticationToken)
    {
    }

    public string? Iban { get; set; }
    public string? Nickname { get; set; }
    public string? Address { get; set; }
}