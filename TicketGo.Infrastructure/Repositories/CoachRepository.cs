using Microsoft.EntityFrameworkCore;
using TicketGo.Domain.Entities;
using TicketGo.Domain.Interfaces;
using TicketGo.Infrastructure.Data;

namespace TicketGo.Infrastructure.Repositories
{
    public class CoachRepository : ICoachRepository
    {
        private readonly AppDbContext _context;

        public CoachRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Coach> GetCoachWithRelatedDataAsync(int idCoach)
        {
            return await _context.Coaches
                .Include(c => c.Seats)
                .Include(c => c.IdTrainNavigation)
                    .ThenInclude(t => t.IdTrainRouteNavigation)
                .FirstOrDefaultAsync(c => c.IdCoach == idCoach);
        }
        
        public async Task<List<Coach>> GetAllAsync()
        {
            return await _context.Coaches
                .Include(c => c.IdTrainNavigation)
                .ToListAsync();
        }

        public async Task<Coach> GetByIdAsync(int id)
        {
            return await _context.Coaches
                .Include(c => c.IdTrainNavigation)
                .FirstOrDefaultAsync(c => c.IdCoach == id);
        }

        public async Task AddAsync(Coach coach)
        {
            await _context.Coaches.AddAsync(coach);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Coach coach)
        {
            _context.Coaches.Update(coach);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var coach = await _context.Coaches.FindAsync(id);
            if (coach != null)
            {
                _context.Coaches.Remove(coach);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Coaches.AnyAsync(c => c.IdCoach == id);
        }
    }
}