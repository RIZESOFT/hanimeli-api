namespace HanimeliApp.Domain.Models.Order;

public class AssigmentOrderModel
{
    public string DayName { get; set; }
    public DateTime Date { get; set; }
    public List<AssigmentOrderHourModel> Hours { get; set; }
}

public class AssigmentOrderHourModel
{
    public string Hour { get; set; }
    public List<AssigmentOrderHourMenuModel> Menus { get; set; }
}

public class AssigmentOrderHourMenuModel
{
    public int MenuId { get; set; }
    public string MenuName { get; set; }
    public int Count { get; set; }
}