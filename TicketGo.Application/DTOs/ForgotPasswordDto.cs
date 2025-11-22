using System.ComponentModel.DataAnnotations;

namespace TicketGo.Application.DTOs
{
    public class ForgotPasswordDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;
    }
}
