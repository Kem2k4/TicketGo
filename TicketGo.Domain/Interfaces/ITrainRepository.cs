using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TicketGo.Domain.Entities; 
using TicketGo.Domain.Common;

namespace TicketGo.Domain.Interfaces
{
    public interface ITrainRepository 
    {
        Task<List<Train>> GetAllAsync();
        Task<PagedResult<Train>> SearchTrainsAsync(
            string noiDi,
            string noiDen,
            DateTime? ngayKhoiHanh,
            string sortTime,
            string sortPrice,
            List<string> loaiXe,
            int page,
            int pageSize);

        Task<Train> GetByIdAsync(int id);
        Task AddAsync(Train train);
        Task UpdateAsync(Train train);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);


        IQueryable<Train> GetQueryable();
    }
}