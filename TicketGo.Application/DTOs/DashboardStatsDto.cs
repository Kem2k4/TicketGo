namespace TicketGo.Application.DTOs
{
    public class DashboardStatsDto
    {
        public int TotalOrders { get; set; }
        public decimal TotalRevenue { get; set; }
        public int TotalTickets { get; set; }
        public int TotalTrains { get; set; }
        public int TotalAccounts { get; set; }
        public List<RevenueByMonthDto> RevenueByMonth { get; set; } = new();
        public List<OrderStatusDto> OrdersByStatus { get; set; } = new();
    }

    public class RevenueByMonthDto
    {
        public string Month { get; set; } = string.Empty;
        public decimal Revenue { get; set; }
    }

    public class OrderStatusDto
    {
        public string Status { get; set; } = string.Empty;
        public int Count { get; set; }
    }
}

