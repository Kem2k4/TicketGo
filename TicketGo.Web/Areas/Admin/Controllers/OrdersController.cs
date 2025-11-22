using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TicketGo.Application.Interfaces;
using TicketGo.Application.DTOs;
using Microsoft.AspNetCore.Authorization;
namespace TicketGo.Web.Areas.Admin.Controllers
{   
    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    public class OrdersController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly IDiscountService _discountService;

        public OrdersController(IOrderService orderService, IDiscountService discountService)
        {
            _discountService = discountService;
            _orderService = orderService;
        }
        
        // [ Lấy danh sách đơn hàng ]
        // GET: Admin/Orders
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var orders = await _orderService.GetAllOrdersAsync();
            return View(orders);
        }

    //     // GET: Admin/Orders/Details/5
    //     public async Task<IActionResult> Details(int? id)
    //     {
    //         if (id == null)
    //         {
    //             return NotFound();
    //         }

    //         var order = await _orderService.GetOrderByIdAsync(id.Value);
    //         if (order == null)
    //         {
    //             return NotFound();
    //         }

    //         return View(order);
    //     }

    //     // GET: Admin/Orders/Create
    //     public async Task<IActionResult> Create()
    //     {
    //         var customers = await _orderService.GetAllCustomersAsync();
    //         var discounts = await _discountService.GetAllDiscountsAsync();

    //         ViewData["IdCus"] = new SelectList(customers, "IdCus", "CustomerName");
    //         ViewData["IdDiscount"] = new SelectList(discounts, "IdDiscount", "DiscountName");
    //         return View();
    //     }

    //     // POST: Admin/Orders/Create
    //     [HttpPost]
    //     [ValidateAntiForgeryToken]
    //     public async Task<IActionResult> Create(CreateUpdateOrderDto orderDto)
    //     {
    //         if (ModelState.IsValid)
    //         {
    //             await _orderService.CreateOrderAsync(orderDto);
    //             return RedirectToAction(nameof(Index));
    //         }

    //         var customers = await _orderService.GetAllCustomersAsync();
    //         var discounts = await _discountService.GetAllDiscountsAsync();

    //         ViewData["IdCus"] = new SelectList(customers, "IdCus", "CustomerName", orderDto.IdCus);
    //         ViewData["IdDiscount"] = new SelectList(discounts, "IdDiscount", "DiscountName", orderDto.IdDiscount);
    //         return View(orderDto);
    //     }

    //     // GET: Admin/Orders/Edit/5
    //     public async Task<IActionResult> Edit(int? id)
    //     {
    //         if (id == null)
    //         {
    //             return NotFound();
    //         }

    //         var order = await _orderService.GetOrderByIdAsync(id.Value);
    //         if (order == null)
    //         {
    //             return NotFound();
    //         }

    //         var customers = await _orderService.GetAllCustomersAsync();
    //         var discounts = await _discountService.GetAllDiscountsAsync();

    //         ViewData["IdCus"] = new SelectList(customers, "IdCus", "CustomerName", order.IdCus);
    //         ViewData["IdDiscount"] = new SelectList(discounts, "IdDiscount", "DiscountName", order.IdDiscount);

    //         var orderDto = new CreateUpdateOrderDto
    //         {
    //             IdOrder = order.IdOrder,
    //             TotalPrice = order.TotalPrice,
    //             DateOrder = order.DateOrder,
    //             IdTicket = order.IdTicket,
    //             IdDiscount = order.IdDiscount,
    //             NameCus = order.NameCus,
    //             Phone = order.Phone,
    //             IdCus = order.IdCus
    //         };

    //         return View(orderDto);
    //     }

    //     // POST: Admin/Orders/Edit/5
    //     [HttpPost]
    //     [ValidateAntiForgeryToken]
    //     public async Task<IActionResult> Edit(int id, CreateUpdateOrderDto orderDto)
    //     {
    //         if (id != orderDto.IdOrder)
    //         {
    //             return NotFound();
    //         }

    //         if (ModelState.IsValid)
    //         {
    //             try
    //             {
    //                 await _orderService.UpdateOrderAsync(id, orderDto);
    //             }
    //             catch (Exception)
    //             {
    //                 if (await _orderService.GetOrderByIdAsync(id) != null)
    //                 {
    //                     return NotFound();
    //                 }
    //                 throw;
    //             }
    //             return RedirectToAction(nameof(Index));
    //         }

    //         var customers = await _orderService.GetAllCustomersAsync();
    //         var discounts = await _discountService.GetAllDiscountsAsync();

    //         ViewData["IdCus"] = new SelectList(customers, "IdCus", "CustomerName", orderDto.IdCus);
    //         ViewData["IdDiscount"] = new SelectList(discounts, "IdDiscount", "DiscountName", orderDto.IdDiscount);
    //         return View(orderDto);
    //     }

    //     // GET: Admin/Orders/Delete/5
    //     public async Task<IActionResult> Delete(int? id)
    //     {
    //         if (id == null)
    //         {
    //             return NotFound();
    //         }

    //         var order = await _orderService.GetOrderByIdAsync(id.Value);
    //         if (order == null)
    //         {
    //             return NotFound();
    //         }

    //         return View(order);
    //     }

    //     // POST: Admin/Orders/Delete/5
    //     [HttpPost, ActionName("Delete")]
    //     [ValidateAntiForgeryToken]
    //     public async Task<IActionResult> DeleteConfirmed(int id)
    //     {
    //         await _orderService.DeleteOrderAsync(id);
    //         return RedirectToAction(nameof(Index));
    //     }
    // }
    }
}