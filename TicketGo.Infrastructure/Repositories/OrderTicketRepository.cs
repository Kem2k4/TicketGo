using Microsoft.EntityFrameworkCore;
using TicketGo.Domain.Entities;
using TicketGo.Domain.Interfaces;
using TicketGo.Infrastructure.Data;

namespace TicketGo.Infrastructure.Repositories
{
    public class OrderTicketRepository : IOrderTicketRepository
    {
        private readonly AppDbContext _context;

        public OrderTicketRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<OrderTicket> GetByIdAsync(int id)
        {
            return await _context.OrderTickets
                .Include(ot => ot.IdOrderNavigation)
                .Include(ot => ot.IdTicketNavigation)
                .FirstOrDefaultAsync(ot => ot.IdOrder == id);
        }

        public async Task AddAsync(OrderTicket orderTicket)
        {
            await _context.OrderTickets.AddAsync(orderTicket);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(OrderTicket orderTicket)
        {
            _context.OrderTickets.Update(orderTicket);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var orderTicket = await _context.OrderTickets
                .FirstOrDefaultAsync(ot => ot.IdOrder == id);
            if (orderTicket != null)
            {
                _context.OrderTickets.Remove(orderTicket);
                await _context.SaveChangesAsync();
            }
        }
    }
}