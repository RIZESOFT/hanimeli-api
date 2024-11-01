namespace HanimeliApp.Domain.Models.User
{
    public class UserLoginResultModel
    {
        public UserLoginResultModel(int id, string firstname, string lastName, string email, string role, string token)
        {
            Id = id;
            FirstName = firstname;
            LastName = lastName;
            Email = email;
            Role = role;
            AuthenticationToken = token;
        }
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string AuthenticationToken { get; set; }
        public string Role { get; set; }
	}
}
