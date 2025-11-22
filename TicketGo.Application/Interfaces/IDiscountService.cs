using TicketGo.Application.DTOs;

namespace TicketGo.Application.Interfaces
{
    public interface IDiscountService
    {
        Task<List<DiscountDto>> GetAllDiscountsAsync();

    }
}