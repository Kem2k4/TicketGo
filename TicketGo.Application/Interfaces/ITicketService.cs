using TicketGo.Application.DTOs;
using X.PagedList;

namespace TicketGo.Application.Interfaces
{
    public interface ITicketService
    {
        Task<IPagedList<TicketDto>> GetPagedTicketsAsync(int pageNumber, int pageSize);
        Task<TicketDto> GetTicketByIdAsync(int id);
        Task UpdateTicketAsync(int id, CreateUpdateTicketDto ticketDto);
        Task DeleteTicketAsync(int id);
        Task<List<SeatDto>> GetAllSeatsAsync();
        Task<List<TrainDto>> GetAllTrainsAsync();
    }
}