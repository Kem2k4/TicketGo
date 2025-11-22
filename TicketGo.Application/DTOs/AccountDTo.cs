using System;
using System.ComponentModel.DataAnnotations;

namespace TicketGo.Application.DTOs
{
    //[Danh sách tài khoản, thêm tài khoản, sửa tài khoản]
    public class AccountDto
    {
        public int? IdAccount { get; set; } = null;
        public string? Fullname { get; set; } = null;
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool? Sex { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public int IdRole { get; set; } = 2; // Mặc định là 2 (khách hàng)
        public string RoleName { get; set; } // Tên vai trò (từ IdRoleNavigation)

        public bool? IsEmailConfirmed { get; set; }
        
    }

    // [Tạo tài khoản mới]
   public class RegisterDto
    {
  
        public string Fullname { get; set; }
        public string Phone { get; set; }   
        public string Email { get; set; }
        public string Password { get; set; }
        [Compare("Password", ErrorMessage = "Mật khẩu nhập lại không khớp")]
        public string ConfirmPassword { get; set; }
        public bool? Sex { get; set; }
        public DateTime? DateOfBirth { get; set; }
    }

    // [Đăng nhập]
    public class LoginDto{
        public string? Email { get; set; }
        public string? Password { get; set; } 
    }
}