using HanimeliApp.Domain.Models.Order;

namespace HanimeliApp.Domain.Models.Cook;

public class CookDashboardModel
{
    public OrderModel? NextOrder { get; set; }
    public decimal BalanceAmount { get; set; }
    public int MonthlyTotalOrderCount { get; set; }
    public List<OrderModel>? DailyOrders { get; set; }
    
}