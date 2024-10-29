namespace HanimeliApp.Domain.Dtos.User;

public class UpdateB2BUserSettingsRequest
{
    public int UserId { get; set; }
    public int? DailyOrderCount { get; set; }
    public int? AvailableWeekDays { get; set; }
    public string? OrderHours { get; set; }
    public string? OrderDays { get; set; }
}