using TicketGo.Domain.Entities;

namespace TicketGo.Domain.Interfaces
{
    public interface IRoleRepository 
    {
        Task<List<Role>> GetAllAsync();
        Task<Role> GetByIdAsync(int id);
        Task AddAsync(Role role);
        Task UpdateAsync(Role role);
        Task DeleteAsync(int id);
    }
}