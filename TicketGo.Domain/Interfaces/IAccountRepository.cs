using TicketGo.Domain.Entities;

namespace TicketGo.Domain.Interfaces
{
    public interface IAccountRepository 
    {   
        Task<List<Account>> GetAllAsync();
        Task<Account> GetByIdAsync(int id);
        Task<Account> GetByEmailAsync(string email);
        Task AddAsync(Account account);
        Task UpdateAsync(Account account);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        
        //
        Task<string> GenerateTokenAsync(Account account);
        Task<bool> VerifyEmailAsync(string email, string token);
    }
}