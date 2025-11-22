using TicketGo.Application.DTOs;
using TicketGo.Domain.Entities;
using X.PagedList;

namespace TicketGo.Application.Interfaces
{
    public interface ISeatService
    {        
        //[Tạo nhiều ghế]
        Task CreateSeatAsync(int coachId, int seatsQuantity);
        //[Xóa nhiều ghế]
        Task DeleteSeatsAsync(int coachId);
    }
}