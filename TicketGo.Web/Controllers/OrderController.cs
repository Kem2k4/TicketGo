using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TicketGo.Application.Interfaces;
using TicketGo.Application.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace TicketGo.Web.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IOrderService _orderService;
        private readonly IVNPayService _vnPayService;

        public OrderController(IOrderService orderService, IVNPayService vnPayService, IHttpContextAccessor httpContext)
        {
            _httpContextAccessor = httpContext;
            _orderService = orderService;
            _vnPayService = vnPayService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int idCoach)
        {
            var viewModel = await _orderService.GetOrderTicketDetailsAsync(idCoach);
            if (viewModel == null)
                return NotFound();

            HttpContext.Session.SetInt32("CoachID", idCoach);
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Index(List<string> listSeats, string Fullname, string Phone, string Email, decimal TotalPrice)
        {
            if (!ModelState.IsValid)
                return NotFound();

            var httpContext = _httpContextAccessor.HttpContext;
            var IdAccount = httpContext.Session.GetInt32("AccountID");
            if (IdAccount == null)
                return NotFound();

            string jsonSeats = JsonConvert.SerializeObject(listSeats);
            HttpContext.Session.SetString("SelectedSeats", jsonSeats);
            HttpContext.Session.SetString("Fullname", Fullname);
            HttpContext.Session.SetString("Phone", Phone);
            HttpContext.Session.SetString("Email", Email);
            HttpContext.Session.SetString("TotalPrice", TotalPrice.ToString());

            string idOrder = DateTime.Now.ToString("yyyyMMdd");
            int orderId = Convert.ToInt32(idOrder);
            var vnPayModel = new VnPayRequestDto
            {
                Amount = (double)TotalPrice,
                CreatedDate = DateTime.Now,
                Description = "Thanh toán",
                Fullname = Fullname,
                OrderId = orderId
            };

            return Redirect(_vnPayService.CreatePaymentUrl(HttpContext, vnPayModel));
        }

        public IActionResult Pay_Fail() => View();
        public IActionResult Pay_Success() => View();

        public async Task<IActionResult> PaymentCallBack()
        {
            var fullname = HttpContext.Session.GetString("Fullname");
            var IdAccount = HttpContext.Session.GetInt32("AccountID");
            var phone = HttpContext.Session.GetString("Phone");
            var email = HttpContext.Session.GetString("Email");
            var totalPrice = decimal.Parse(HttpContext.Session.GetString("TotalPrice"));
            var idCoach = HttpContext.Session.GetInt32("CoachID");

            string jsonSeats = HttpContext.Session.GetString("SelectedSeats");
            List<string> listSeats = JsonConvert.DeserializeObject<List<string>>(jsonSeats);
            string listSeatsFinal = listSeats.First();
            List<string> seats = JsonConvert.DeserializeObject<List<string>>(listSeatsFinal);

            var createOrderDto = new OrderDto
            {
                ListSeats = seats,
                NameCus = fullname,
                Phone = phone,
                TotalPrice = (double)totalPrice,
                IdCoach = idCoach,
                IdAccount = IdAccount
            };

            await _orderService.CreateOrderAsync(createOrderDto);

            TempData["Message"] = "Thanh toán thành công";
            return RedirectToAction("Pay_Success");
        }

        [HttpGet]
        public async Task<IActionResult> MyOrders()
        {
            var accountId = HttpContext.Session.GetInt32("AccountID");
            if (accountId == null)
            {
                return RedirectToAction("Login", "Access");
            }

            var orders = await _orderService.GetOrdersByAccountAsync(accountId.Value);
            return View(orders);
        }
    }
}
