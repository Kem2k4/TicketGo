using TicketGo.Application.DTOs;

namespace TicketGo.Application.Interfaces
{
    public interface IOrderService
    {
        Task<OrderTicketDto> GetOrderTicketDetailsAsync(int idCoach);
        Task<List<OrderDto>> GetAllOrdersAsync();
        Task<OrderDto> GetOrderByIdAsync(int id);
        Task<List<OrderDto>> GetOrdersByAccountAsync(int accountId);
        Task CreateOrderAsync(OrderDto orderDto);
        Task UpdateOrderAsync(int id, OrderDto orderDto);
        Task DeleteOrderAsync(int id);
        Task<DashboardStatsDto> GetDashboardStatsAsync();
    }
}