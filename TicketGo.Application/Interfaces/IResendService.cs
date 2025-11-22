
namespace TicketGo.Application.Interfaces
{
    public interface IResendService
    {
        //[Gửi email xác thực]
        Task SendVerificationEmailAsync(string email, string verifyUrl);
    }
}