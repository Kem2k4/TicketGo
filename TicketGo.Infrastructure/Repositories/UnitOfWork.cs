using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage;
using TicketGo.Infrastructure.Data;
using TicketGo.Domain.Interfaces;
using TicketGo.Application.Interfaces;

namespace TicketGo.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private IDbContextTransaction _transaction;

        public ICoachRepository CoachRepository { get; }
        public ISeatRepository SeatRepository { get; }

        public UnitOfWork(AppDbContext context, ICoachRepository coachRepository, ISeatRepository seatRepository)
        {
            _context = context;
            CoachRepository = coachRepository;
            SeatRepository = seatRepository;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
            return _transaction;
        }

        public async Task CommitAsync()
        {
            if (_transaction != null)
            {
                await _transaction.CommitAsync();
                _transaction.Dispose();
                _transaction = null;
            }
        }

        public async Task RollbackAsync()
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
                _transaction.Dispose();
                _transaction = null;
            }
        }
    }
}