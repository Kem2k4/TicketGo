using Microsoft.EntityFrameworkCore;
using TicketGo.Domain.Entities;
using TicketGo.Domain.Interfaces;
using TicketGo.Infrastructure.Data;

namespace TicketGo.Infrastructure.Repositories
{
    public class SeatRepository : ISeatRepository
    {
        private readonly AppDbContext _context;

        public SeatRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Seat>> GetAllAsync()
        {
            return await _context.Seats
                .Include(s => s.IdCoachNavigation)
                .Include(s => s.Tickets)
                .ToListAsync();
        }

        public async Task<Seat> GetByNameAndCoachIdAsync(string nameSeat, int idCoach)
        {
            if (string.IsNullOrWhiteSpace(nameSeat))
            {
                throw new ArgumentException("Seat name cannot be null or empty.");
            }

            var seat = await _context.Seats
                .Include(s => s.IdCoachNavigation)
                .Include(s => s.Tickets)
                .FirstOrDefaultAsync(s => s.NameSeat == nameSeat && s.IdCoach == idCoach);

            if (seat == null)
            {
                throw new KeyNotFoundException($"Seat with name '{nameSeat}' and Coach ID {idCoach} not found.");
            }

            return seat;
        }

        public async Task<Seat> GetByIdAsync(int id)
        {
            var seat = await _context.Seats
                .Include(s => s.IdCoachNavigation)
                .Include(s => s.Tickets)
                .FirstOrDefaultAsync(s => s.IdSeat == id);

            if (seat == null)
            {
                throw new KeyNotFoundException($"Seat with ID {id} not found.");
            }

            return seat;
        }

        public async Task AddAsync(Seat seat)
        {
            if (seat == null || string.IsNullOrWhiteSpace(seat.NameSeat))
            {
                throw new ArgumentException("Seat cannot be null and must have a valid name.");
            }

            await _context.Seats.AddAsync(seat);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Seat seat)
        {
            if (seat == null || string.IsNullOrWhiteSpace(seat.NameSeat))
            {
                throw new ArgumentException("Seat cannot be null and must have a valid name.");
            }

            var existingSeat = await _context.Seats.FindAsync(seat.IdSeat);
            if (existingSeat == null)
            {
                throw new KeyNotFoundException($"Seat with ID {seat.IdSeat} not found.");
            }

            _context.Entry(existingSeat).CurrentValues.SetValues(seat);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var seat = await _context.Seats
                .Include(s => s.Tickets)
                .FirstOrDefaultAsync(s => s.IdSeat == id);

            if (seat == null)
            {
                throw new KeyNotFoundException($"Seat with ID {id} not found.");
            }

            if (seat.Tickets.Any())
            {
                throw new InvalidOperationException($"Cannot delete seat with ID {id} because it is associated with one or more tickets.");
            }

            _context.Seats.Remove(seat);
            await _context.SaveChangesAsync();
        }
        // [Xóa nhiều ghế]
        public async Task DeleteByCoachIdAsync(int coachId)
        {
            var seats = await _context.Seats
                .Include(s => s.Tickets)
                .Where(s => s.IdCoach == coachId)
                .ToListAsync();

            if (!seats.Any())
            {
                throw new KeyNotFoundException($"No seats found for Coach ID {coachId}.");
            }

            if (seats.Any(s => s.Tickets.Any()))
            {
                throw new InvalidOperationException($"Cannot delete seats for Coach ID {coachId} because one or more seats are associated with tickets.");
            }

            _context.Seats.RemoveRange(seats);
            await _context.SaveChangesAsync();
        }

        //[Thêm nhiều ghế]
        public async Task AddRangeAsync(IEnumerable<Seat> seats)
        {
            await _context.Seats.AddRangeAsync(seats);
        }

    }
}