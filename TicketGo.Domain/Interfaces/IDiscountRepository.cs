using TicketGo.Domain.Entities;

namespace TicketGo.Domain.Interfaces
{
    public interface IDiscountRepository
    {
        Task<List<Discount>> GetAllAsync();
        // Task<Discount> GetByIdAsync(int id);
        // Task AddAsync(Discount discount);
        // Task UpdateAsync(Discount discount);
        // Task DeleteAsync(int id);
    }
}