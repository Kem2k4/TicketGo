using TicketGo.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using TicketGo.Domain.Interfaces;
using TicketGo.Infrastructure.Data;
using X.PagedList;


namespace TicketGo.Infrastructure.Repositories
{
    public class TicketRepository : ITicketRepository
    {
        private readonly AppDbContext _context;

        public TicketRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IPagedList<Ticket>> GetPagedTicketsAsync(int pageNumber, int pageSize)
        {
            if (pageNumber < 1 || pageSize < 1)
            {
                throw new ArgumentException("Page number and page size must be greater than zero.");
            }

            var query = _context.Tickets
                .Include(t => t.IdSeatNavigation)
                .Include(t => t.IdTrainNavigation)
                .OrderBy(t => t.IdTicket);

            var totalItemCount = await query.CountAsync();

            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var pagedList = new StaticPagedList<Ticket>(items, pageNumber, pageSize, totalItemCount);

            return pagedList;
        }

        public async Task<Ticket> GetByIdAsync(int id)
        {
            var ticket = await _context.Tickets
                .Include(t => t.IdSeatNavigation)
                .Include(t => t.IdTrainNavigation)
                .FirstOrDefaultAsync(t => t.IdTicket == id);

            if (ticket == null)
            {
                throw new KeyNotFoundException($"Ticket with ID {id} not found.");
            }

            return ticket;
        }

        public async Task AddAsync(Ticket ticket)
        {
            if (ticket == null || ticket.IdSeat == 0 || ticket.IdTrain == 0)
            {
                throw new ArgumentException("Ticket cannot be null and must have valid Seat and Train IDs.");
            }

            await _context.Tickets.AddAsync(ticket);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Ticket ticket)
        {
            if (ticket == null || ticket.IdSeat == 0 || ticket.IdTrain == 0)
            {
                throw new ArgumentException("Ticket cannot be null and must have valid Seat and Train IDs.");
            }

            var existingTicket = await _context.Tickets.FindAsync(ticket.IdTicket);
            if (existingTicket == null)
            {
                throw new KeyNotFoundException($"Ticket with ID {ticket.IdTicket} not found.");
            }

            _context.Entry(existingTicket).CurrentValues.SetValues(ticket);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var ticket = await _context.Tickets
                .Include(t => t.OrderTickets)
                .FirstOrDefaultAsync(t => t.IdTicket == id);

            if (ticket == null)
            {
                throw new KeyNotFoundException($"Ticket with ID {id} not found.");
            }

            if (ticket.OrderTickets.Any())
            {
                throw new InvalidOperationException($"Cannot delete ticket with ID {id} because it is associated with one or more orders.");
            }

            _context.Tickets.Remove(ticket);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Tickets.AnyAsync(t => t.IdTicket == id);
        }
    }
}