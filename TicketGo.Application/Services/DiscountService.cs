using TicketGo.Application.DTOs;
using TicketGo.Domain.Entities;
using TicketGo.Domain.Interfaces;
using TicketGo.Application.Interfaces;

namespace TicketGo.Application.Services
{
    public class DiscountService : IDiscountService
    {
        private readonly IDiscountRepository _discountRepository;

        public DiscountService(IDiscountRepository discountRepository)
        {
            _discountRepository = discountRepository;
        }
        
        public async Task<List<DiscountDto>> GetAllDiscountsAsync()
        {
            var discounts = await _discountRepository.GetAllAsync();
            return discounts.Select(d => new DiscountDto
            {
                IdDiscount = d.IdDiscount,
                DiscountName = d.IdDiscount.ToString() // Có thể thay bằng thuộc tính phù hợp của Discount
            }).ToList();
        }
    }
}