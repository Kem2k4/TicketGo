using TicketGo.Domain.Entities;
using TicketGo.Application.DTOs;

namespace TicketGo.Application.Interfaces
{
    public interface ICoachService
    {
        Task<List<CoachDto>> GetAllCoachesAsync();
        Task<CoachDto> GetCoachByIdAsync(int id);
        Task CreateCoachAsync(CoachDto coachDto);
        Task UpdateCoachAsync(int id, CoachDto coachDto);
        Task DeleteCoachAsync(int id);
        Task<List<TrainDto>> GetAllTrainsAsync();
    }
}