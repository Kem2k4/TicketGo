using TicketGo.Domain.Entities;
using TicketGo.Application.DTOs;
using Microsoft.AspNetCore.Http;

namespace TicketGo.Application.Interfaces
{
    public interface IAccountService
    {
        //[Đăng ký tài khoản]
        Task<bool> RegisterAsync(RegisterDto registerDto);
        //[Đăng nhập tài khoản]
        Task<Account> LoginAsync(string email, string password);
        //[Danh sách tài khoản]
        Task<List<AccountDto>> GetAllAccountsAsync();
        //[Chi tiết tài khoản]
        Task<AccountDto> GetAccountByIdAsync(int id);
        //[Tìm tài khoản theo email]
        Task<Account> GetByEmailAsync(string email);
        //[Tạo tài khoản]
        Task CreateAccountAsync(AccountDto accountDto);
        //[Cập nhật tài khoản]
        Task UpdateAccountAsync(int id, AccountDto accountDto);
        //[Xóa tài khoản]
        Task DeleteAccountAsync(int id);
        Task<bool> VerifyEmailAsync(VerifyEmailDto verifyEmailDto, HttpContext httpContext);
        string GenerateVerificationCode();
        Task<List<RoleDto>> GetAllRolesAsync();


        //
        Task<string> GenerateTokenAsync(Account account);
        Task<bool> VerifyEmailAsync(string email, string token);
    }
}