namespace HanimeliApp.Domain.Models.User
{
    public class UserLoginResultModel
    {
        public UserLoginResultModel(string firstname, string lastName, string email, string token)
        {
            FirstName = firstname;
            LastName = lastName;
            Email = email;
            AuthenticationToken = token;
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string AuthenticationToken { get; set; }
	}
}
