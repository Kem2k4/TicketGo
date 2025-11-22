using TicketGo.Domain.Entities;
namespace TicketGo.Domain.Interfaces
{
    public interface ICoachRepository
    {
        Task<Coach> GetCoachWithRelatedDataAsync(int idCoach);
        Task<Coach> GetByIdAsync(int id);
        Task<List<Coach>> GetAllAsync();
        Task AddAsync(Coach coach);
        Task UpdateAsync(Coach coach);
        Task DeleteAsync(int id);
    }
}