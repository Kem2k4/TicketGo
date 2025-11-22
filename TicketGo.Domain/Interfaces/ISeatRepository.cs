using TicketGo.Domain.Entities;

namespace TicketGo.Domain.Interfaces
{
    public interface ISeatRepository 
    {
        Task<List<Seat>> GetAllAsync();
        Task<Seat> GetByNameAndCoachIdAsync(string nameSeat, int idCoach);
        Task<Seat> GetByIdAsync(int id);
        Task AddAsync(Seat seat);
        Task UpdateAsync(Seat seat);
        Task DeleteAsync(int id);
        //[Thêm nhiều ghế]
        Task AddRangeAsync(IEnumerable<Seat> seats);
        //[Xóa nhiều ghế]
        Task DeleteByCoachIdAsync(int coachId);
    }
}