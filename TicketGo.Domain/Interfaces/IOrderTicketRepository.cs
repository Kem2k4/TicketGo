using TicketGo.Domain.Entities;

namespace TicketGo.Domain.Interfaces
{
    public interface IOrderTicketRepository 
    {
        Task<OrderTicket> GetByIdAsync(int id);
        Task AddAsync(OrderTicket orderTicket);
        Task UpdateAsync(OrderTicket orderTicket);
        Task DeleteAsync(int id);
    }
}