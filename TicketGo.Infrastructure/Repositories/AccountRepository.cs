using Microsoft.EntityFrameworkCore;
using TicketGo.Domain.Entities;
using TicketGo.Domain.Interfaces;
using TicketGo.Infrastructure.Data;

namespace TicketGo.Infrastructure.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly AppDbContext _context;

        public AccountRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Account>> GetAllAsync()
        {
            return await _context.Accounts
                .Include(a => a.IdRoleNavigation)
                .ToListAsync();
        }

        public async Task<Account> GetByIdAsync(int id)
        {
            return await _context.Accounts
                .Include(a => a.IdRoleNavigation)
                .FirstOrDefaultAsync(a => a.IdAccount == id);
        }

        public async Task AddAsync(Account account)
        {
            await _context.Accounts.AddAsync(account);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Account account)
        {
            _context.Accounts.Update(account);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var account = await _context.Accounts.FindAsync(id);
            if (account != null)
            {
                _context.Accounts.Remove(account);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Account> GetByEmailAsync(string email)
        {
            return await _context.Accounts.FirstOrDefaultAsync(x => x.Email == email);
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Accounts.AnyAsync(a => a.IdAccount == id);
        }

    public async Task<string> GenerateTokenAsync(Account account)
    {
        var tokenValue = Guid.NewGuid().ToString();

        var token = new Token
        {
            Value = tokenValue,
            ExpiredAt = DateTime.UtcNow.AddHours(24),
            AccountId = account.IdAccount
        };

        _context.Tokens.Add(token);
        await _context.SaveChangesAsync();

        return tokenValue;
    }

    public async Task<bool> VerifyEmailAsync(string email, string tokenValue)
    {
        var account = await _context.Accounts
            .Include(a => a.Tokens)
            .FirstOrDefaultAsync(a => a.Email == email);

        if (account == null)
            return false;

        var token = account.Tokens.FirstOrDefault(t => t.Value == tokenValue && t.ExpiredAt > DateTime.UtcNow);
        if (token == null)
            return false;

        account.IsEmailConfirmed = true;
        _context.Tokens.Remove(token);

        await _context.SaveChangesAsync();
        return true;
    }

    }
}