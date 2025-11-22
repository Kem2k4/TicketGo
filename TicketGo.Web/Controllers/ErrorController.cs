using Microsoft.AspNetCore.Mvc;

namespace TicketGo.Web.Controllers
{
    [Route("Error")]
    public class ErrorController : Controller
    {
        [Route("404")]
        public IActionResult Error404() => View("NotFound");

        [Route("500")]
        public IActionResult Error500() => View("InternalServer");

        [Route("403")]
        public IActionResult Error403() => View("Forbidden");
    }
}
