using Microsoft.EntityFrameworkCore;
using TicketGo.Domain.Interfaces;
using TicketGo.Domain.Entities;
using TicketGo.Infrastructure.Data;

namespace TicketGo.Infrastructure.Repositories
{
    public class TrainRouteRepository : ITrainRouteRepository
    {
        private readonly AppDbContext _context;

        public TrainRouteRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<string>> GetStartPointsAsync(string term)
        {
            var query = _context.TrainRoutes.AsQueryable();

            if (!string.IsNullOrEmpty(term))
            {
                query = query.Where(r => r.PointStart.Contains(term));
            }

            return await query.Select(r => r.PointStart)
                              .Distinct()
                              .ToListAsync();
        }

        public async Task<List<string>> GetEndPointsAsync(string term)
        {
            var query = _context.TrainRoutes.AsQueryable();

            if (!string.IsNullOrEmpty(term))
            {
                query = query.Where(r => r.PointEnd.Contains(term));
            }

            return await query.Select(r => r.PointEnd)
                              .Distinct()
                              .ToListAsync();
        }
        
        public async Task<List<TrainRoute>> GetAllAsync()
        {
            return await _context.TrainRoutes.ToListAsync();
        }

        public async Task<TrainRoute> GetByIdAsync(int id)
        {
            return await _context.TrainRoutes.FirstOrDefaultAsync(tr => tr.IdTrainRoute == id);
        }

        public async Task AddAsync(TrainRoute trainRoute)
        {
            await _context.TrainRoutes.AddAsync(trainRoute);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(TrainRoute trainRoute)
        {
            _context.TrainRoutes.Update(trainRoute);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var trainRoute = await _context.TrainRoutes.FindAsync(id);
            if (trainRoute != null)
            {
                _context.TrainRoutes.Remove(trainRoute);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.TrainRoutes.AnyAsync(tr => tr.IdTrainRoute == id);
        }
    }
}