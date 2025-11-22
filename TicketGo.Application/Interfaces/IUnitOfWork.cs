using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage;
using TicketGo.Domain.Interfaces;

namespace TicketGo.Application.Interfaces
{
    public interface IUnitOfWork
    {
        ICoachRepository CoachRepository { get; }
        ISeatRepository SeatRepository { get; }
        Task SaveChangesAsync();
        Task<IDbContextTransaction> BeginTransactionAsync(); // Sửa kiểu trả về
        Task CommitAsync();
        Task RollbackAsync();
    }
}