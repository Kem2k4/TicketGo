using Resend;
using TicketGo.Application.Interfaces;

public class ResendService :   IResendService
{
    private readonly IResend _resend;


    public ResendService( IResend resend )
    {
        _resend = resend;
    }
    //[Thực hiện gửi email]
    public async Task Executes(string recipient, string subject, string body, string sender="TicketGo@kiet.site")
    {
        var message = new EmailMessage();
        message.From = sender;
        message.To.Add( recipient );
        message.Subject = subject;
        message.HtmlBody = body;

        await _resend.EmailSendAsync( message );
    }
    // [Gửi email xác thực]
    public async Task SendVerificationEmailAsync(string email, string verifyUrl)
    {
        var subject = "Verify Your Email for TicketGo";
        var body = $"<h1>Email Verification</h1>" +
                    $"<p>Thank you for registering with TicketGo!</p>" +
                    $"<p>Please verify your email by clicking the link below:</p>" +
                    $"<a href='{verifyUrl}' style='padding: 10px 20px; background-color: #007bff; color: white; text-decoration: none; border-radius: 5px;'>Verify Email</a>" +
                    $"<p>If you did not register, please ignore this email.</p>" +
                    $"<p>Link expires in 24 hours.</p>";

        await Executes(email, subject, body);
    }
}