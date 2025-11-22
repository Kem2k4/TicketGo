using Microsoft.AspNetCore.Mvc;
using TicketGo.Application.Interfaces;
using TicketGo.Application.DTOs;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace TicketGo.Web.Controllers
{
    public class AccessController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly IResendService _resendService;

        public AccessController(IAccountService accountService, IResendService resendService)
        {
            _accountService = accountService;
            _resendService = resendService;
        }

        // Đăng ký
        [HttpGet]
        public IActionResult Register() => View();

        // Đăng ký
        [HttpPost]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            var existingAccount = await _accountService.GetAllAccountsAsync();

            if (existingAccount.Any(a => a.Email == registerDto.Email && a.IsEmailConfirmed == true))
            {
                ModelState.AddModelError("Email", "Email đã tồn tại.");
                return View(registerDto);
            }

            if (!ModelState.IsValid)
                return View(registerDto);

            var success = await _accountService.RegisterAsync(registerDto);
            if (success)
            {
                var account = await _accountService.GetByEmailAsync(registerDto.Email);
                if (account != null)
                {
                    var token = await _accountService.GenerateTokenAsync(account);
                    var verifyUrl = Url.Action("VerifyEmail", "Access", new { email = account.Email, token }, protocol: Request.Scheme);
                    await _resendService.SendVerificationEmailAsync(registerDto.Email, verifyUrl);

                    TempData["SuccessMessage"] = "Đăng ký thành công! Vui lòng kiểm tra email để xác minh tài khoản.";
                    return RedirectToAction("Login");
                }
            }

            ViewBag.Message = "Đăng ký thất bại. Vui lòng thử lại.";
            return View(registerDto);
        }

        // Đăng nhập
        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            var myUser = await _accountService.LoginAsync(loginDto.Email, loginDto.Password);
            if (myUser == null)
            {
                ViewBag.Message = "Email hoặc mật khẩu không đúng.";
                return View(loginDto);
            }

            if (!myUser.IsEmailConfirmed)
            {
                ViewBag.Message = "Vui lòng xác thực email trước khi đăng nhập.";
                return View(loginDto);
            }

            // Gán Role 
            string roleName = myUser.IdRole switch
            {
                1 => "Admin",
                2 => "Customer",
                _ => "Guest"
            };

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, myUser.Email),
                new Claim(ClaimTypes.Role, roleName),
                new Claim("UserID", myUser.IdAccount.ToString())
            };

            var identity = new ClaimsIdentity(claims, "MyCookieAuth");
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync("MyCookieAuth", principal, new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTime.UtcNow.AddMinutes(30)
            });


            // Lưu session 
            HttpContext.Session.SetString("UserSession", myUser.Email);
            HttpContext.Session.SetInt32("AccountID", myUser.IdAccount);

            // Điều hướng dựa trên Role
            if (roleName == "Admin")
                return RedirectToAction("Dashboard", "Dashboard", new { area = "Admin" });

            return RedirectToAction("TrangChu", "Home");
        }

        // Xác thực email
        [HttpGet]
        public async Task<IActionResult> VerifyEmail(string email, string token)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(token))
            {
                TempData["ErrorMessage"] = "Liên kết xác thực không hợp lệ.";
                return RedirectToAction("Login");
            }

            var success = await _accountService.VerifyEmailAsync(email, token);
            TempData["SuccessMessage"] = success
                ? "Xác thực email thành công! Bạn đã có thể đăng nhập."
                : "Xác thực email thất bại. Liên kết không hợp lệ hoặc hết hạn.";

            return RedirectToAction("Login");
        }

        // Đăng xuất
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("MyCookieAuth"); 
            HttpContext.Session.Clear();                   
            return RedirectToAction("TrangChu", "Home");
        }

    }
}
