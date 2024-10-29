namespace HanimeliApp.Domain.Models.User;

public class UserModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public int? DailyOrderCount { get; set; }
    public int? AvailableWeekDays { get; set; }
    public List<string>? OrderHours { get; set; }
    public List<int>? OrderDays { get; set; }
}