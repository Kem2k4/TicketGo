using Microsoft.EntityFrameworkCore;
using TicketGo.Domain.Entities;
using TicketGo.Domain.Interfaces;
using TicketGo.Infrastructure.Data;
using TicketGo.Domain.Common;

namespace TicketGo.Infrastructure.Repositories
{
    public class TrainRepository :  ITrainRepository
    {
        private readonly AppDbContext _context;

        public TrainRepository(AppDbContext context)
        {
            _context = context;
        }
        
        public async Task<List<Train>> GetAllAsync()
        {
            return await _context.Trains.ToListAsync();
        }

        public async Task<PagedResult<Train>> SearchTrainsAsync(
            string noiDi,
            string noiDen,
            DateTime? ngayKhoiHanh,
            string sortTime,
            string sortPrice,
            List<string> loaiXe,
            int page,
            int pageSize)
        {
            // Truy vấn cơ bản, bao gồm TrainRoute và Coaches
            var query = _context.Trains
                .Include(t => t.IdTrainRouteNavigation)
                .Include(t => t.Coaches)
                .AsQueryable();

            // Áp dụng tiêu chí tìm kiếm
            if (!string.IsNullOrEmpty(noiDi))
                query = query.Where(t => t.IdTrainRouteNavigation.PointStart.Contains(noiDi));

            if (!string.IsNullOrEmpty(noiDen))
                query = query.Where(t => t.IdTrainRouteNavigation.PointEnd.Contains(noiDen));

            if (ngayKhoiHanh.HasValue)
                query = query.Where(t => t.DateStart.HasValue &&
                    t.DateStart.Value.Date == ngayKhoiHanh.Value.Date);

            // Áp dụng bộ lọc LoaiXe (Category của Coach) - case-insensitive và normalize
            if (loaiXe != null && loaiXe.Any())
            {
                // Normalize filter values: "Ghe" -> "Ghế ngồi", "Giuong" -> "Giường nằm", "Limousine" -> "Limousine"
                var normalizedFilters = loaiXe.Select(f => 
                {
                    var lower = f?.ToLower().Trim() ?? "";
                    return lower switch
                    {
                        "ghe" => "Ghế ngồi",
                        "ghế ngồi" => "Ghế ngồi",
                        "giuong" => "Giường nằm",
                        "giường nằm" => "Giường nằm",
                        "limousine" => "Limousine",
                        _ => f?.Trim() ?? "" // Giữ nguyên nếu không match
                    };
                }).Where(nf => !string.IsNullOrEmpty(nf)).Distinct().ToList();
                
                // Sau khi normalize, chỉ cần dùng Contains đơn giản
                query = query.Where(t => t.Coaches.Any(c => 
                    c.Category != null && normalizedFilters.Contains(c.Category)
                ));
            }

            // Áp dụng bộ lọc SortTime
            if (!string.IsNullOrEmpty(sortTime))
            {
                if (sortTime == "time:asc")
                    query = query.OrderBy(t => t.DateStart);
                else if (sortTime == "time:desc")
                    query = query.OrderByDescending(t => t.DateStart);
            }

            // Áp dụng bộ lọc SortPrice (dựa trên BasicPrice của Coach)
            if (!string.IsNullOrEmpty(sortPrice))
            {
                if (sortPrice == "fare:asc")
                    query = query.OrderBy(t => t.Coaches.Min(c => c.BasicPrice));
                else if (sortPrice == "fare:desc")
                    query = query.OrderByDescending(t => t.Coaches.Max(c => c.BasicPrice));
            }

            // Lấy tổng số bản ghi
            var totalRecords = await query.CountAsync();

            // Áp dụng phân trang
            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            // Trả về kết quả phân trang
            return new PagedResult<Train>
            {
                Items = items,
                Page = page,
                PageSize = pageSize,
                TotalRecords = totalRecords
            };
        }

        public async Task<Train> GetByIdAsync(int id)
        {
            return await _context.Trains
                .Include(t => t.IdTrainRouteNavigation)
                .FirstOrDefaultAsync(t => t.IdTrain == id);
        }
        public async Task AddAsync(Train train)
        {
            await _context.Trains.AddAsync(train);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Train train)
        {
            _context.Trains.Update(train);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var train = await _context.Trains.FindAsync(id);
            if (train != null)
            {
                _context.Trains.Remove(train);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Trains.AnyAsync(t => t.IdTrain == id);
        }



        public IQueryable<Train> GetQueryable()
        {
            return _context.Trains.Include(t => t.IdTrainRouteNavigation);
        }
    }
    
}