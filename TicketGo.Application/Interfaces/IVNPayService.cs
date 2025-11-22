using TicketGo.Domain.Entities;
using Microsoft.AspNetCore.Http;
using TicketGo.Application.DTOs;

namespace TicketGo.Application.Interfaces
{
    public interface IVNPayService
    {
        string CreatePaymentUrl(HttpContext httpContext, VnPayRequestDto model);
        VnPaymentResponseDto PaymentExecute(IQueryCollection collections);
    }
}
