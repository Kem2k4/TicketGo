using System.Collections.Generic;
using System.Threading.Tasks;
using TicketGo.Application.Interfaces;
using TicketGo.Domain.Entities;
using TicketGo.Domain.Interfaces;
namespace TicketGo.Application.Services
{
    public class SeatService : ISeatService
    {
        private readonly ISeatRepository _seatRepository;
        private readonly IUnitOfWork _unitOfWork;

        public SeatService(ISeatRepository seatRepository, IUnitOfWork unitOfWork)
        {
            _seatRepository = seatRepository;
            _unitOfWork = unitOfWork;
        }

        // [Tạo ghế]
        public async Task CreateSeatAsync(int coachId, int seatsQuantity)
        {
            var seats = new List<Seat>();
            for (int i = 1; i <= seatsQuantity; i++)
            {
                seats.Add(new Seat
                {
                    NameSeat = $"Seat_{i}",
                    State = false,
                    IdCoach = coachId
                });
            }

            await _seatRepository.AddRangeAsync(seats);
            await _unitOfWork.SaveChangesAsync();
        }

        // [Xóa nhiều ghế]
        public async Task DeleteSeatsAsync(int coachId)
        {
            await _seatRepository.DeleteByCoachIdAsync(coachId);
            await _unitOfWork.SaveChangesAsync();
        }

    }
}