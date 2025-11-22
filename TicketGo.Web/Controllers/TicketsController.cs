using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TicketGo.Web.Controllers
{
    [Authorize(AuthenticationSchemes = "MyCookieAuth", Roles = "Customer")]
    public class TicketsController : Controller
    {
        public IActionResult ManagerTicket()
        {
            return View();
        }
    }
}
