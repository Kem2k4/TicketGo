using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using TicketGo.Application.DTOs;
using TicketGo.Application.Interfaces;

namespace TicketGo.Web.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    public class AccountsController : Controller
    {
        private readonly IAccountService _accountService;

        public AccountsController(IAccountService accountService)
        {
            _accountService = accountService;
        }
        //[Danh sách tài khoản]
        // GET: Admin/Accounts
        public async Task<IActionResult> Index()
        {
            var accounts = await _accountService.GetAllAccountsAsync();
            return View(accounts);
        }

        //[Tạo tài khoản]
        // POST: Admin/Accounts/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] AccountDto accountDto)
        {
            // Server-side validation
            if (string.IsNullOrWhiteSpace(accountDto.Phone) || string.IsNullOrWhiteSpace(accountDto.Email) || string.IsNullOrWhiteSpace(accountDto.Password))
            {
                ModelState.AddModelError("", "Số điện thoại, email và mật khẩu không được để trống");
            }
            else if (!accountDto.Phone.All(c => char.IsDigit(c)) || accountDto.Phone.Length != 10)
            {
                ModelState.AddModelError("Phone", "Số điện thoại phải gồm 10 chữ số");
            }
            else if (!System.Text.RegularExpressions.Regex.IsMatch(accountDto.Email, @"^[^\s@]+@[^\s@]+\.[^\s@]+$"))
            {
                ModelState.AddModelError("Email", "Email không hợp lệ");
            }
            else if (accountDto.Password.Length < 6)
            {
                ModelState.AddModelError("Password", "Mật khẩu phải ít nhất 6 ký tự");
            }
            else if (!accountDto.DateOfBirth.HasValue)
            {
                ModelState.AddModelError("DateOfBirth", "Ngày sinh là bắt buộc");
            }
            else if (!new[] { "customer", "admin" }.Contains(accountDto.RoleName?.ToLower()))
            {
                ModelState.AddModelError("RoleName", "Vai trò phải là Customer hoặc Admin");
            }

            if (!ModelState.IsValid)
            {
                var accounts = await _accountService.GetAllAccountsAsync();
                return View("Index", accounts);
            }

            try
            {
                await _accountService.CreateAccountAsync(accountDto);
                TempData["SuccessMessage"] = "Tài khoản đã được thêm!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Lỗi khi tạo tài khoản: " + ex.Message);
                return RedirectToAction(nameof(Index));
            }
        }
        
        //[Chỉnh sửa tài khoản]
        // POST: Admin/Accounts/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromForm] AccountDto accountDto)
        {
            // Server-side validation
            if (string.IsNullOrWhiteSpace(accountDto.Phone) || string.IsNullOrWhiteSpace(accountDto.Email))
            {
                ModelState.AddModelError("", "Số điện thoại và email không được để trống");
            }
            else if (!accountDto.Phone.All(c => char.IsDigit(c)) || accountDto.Phone.Length != 10)
            {
                ModelState.AddModelError("Phone", "Số điện thoại phải gồm 10 chữ số");
            }
            else if (!System.Text.RegularExpressions.Regex.IsMatch(accountDto.Email, @"^[^\s@]+@[^\s@]+\.[^\s@]+$"))
            {
                ModelState.AddModelError("Email", "Email không hợp lệ");
            }
            else if (!accountDto.DateOfBirth.HasValue)
            {
                ModelState.AddModelError("DateOfBirth", "Ngày sinh là bắt buộc");
            }
            else if (!new[] { "customer", "admin" }.Contains(accountDto.RoleName?.ToLower()))
            {
                ModelState.AddModelError("RoleName", "Vai trò phải là Customer hoặc Admin");
            }

            if (!ModelState.IsValid)
            {
                return RedirectToAction(nameof(Index));
            }

            try
            {
                var existingAccount = await _accountService.GetAccountByIdAsync(accountDto.IdAccount.Value);
                if (existingAccount == null)
                {
                    return NotFound();
                }

                await _accountService.UpdateAccountAsync(accountDto.IdAccount.Value, accountDto);
                TempData["SuccessMessage"] = "Tài khoản đã được cập nhật!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Lỗi khi cập nhật tài khoản: " + ex.Message);
                return RedirectToAction(nameof(Index));
            }
        }

        //[Xóa tài khoản]
        // POST: Admin/Accounts/Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {

            var account = await _accountService.GetAccountByIdAsync(id);
            if (account == null)
            {
                return NotFound();
            }

            try
            {
                await _accountService.DeleteAccountAsync(id);
                TempData["SuccessMessage"] = "Tài khoản đã được xóa!";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Lỗi khi xóa tài khoản: " + ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }
        //Đăng xuất 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Clear();
            await HttpContext.SignOutAsync("MyCookieAuth");
            return RedirectToAction("TrangChu", "Home", new { area = "" });
        }
    }
}