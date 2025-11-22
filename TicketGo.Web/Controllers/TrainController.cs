using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TicketGo.Application.DTOs;
using TicketGo.Application.Interfaces;

namespace TicketGo.Web.Controllers
{
    public class TrainController : Controller
    {
        private readonly ITrainService _trainService;

        public TrainController(ITrainService trainService)
        {
            _trainService = trainService;
        }

        // Danh sách chuyến tàu (ai cũng xem được)
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ListTrain([FromQuery] TrainSearchRequest request)
        {
            var result = await _trainService.SearchTrainsAsync(request);
            return View("ListTrain", result);
        }

        // Gợi ý điểm xuất phát
        [HttpGet("start-points")]
        [AllowAnonymous]
        public async Task<IActionResult> GetStartPoints(string term) =>
            Ok(await _trainService.GetStartPointsAsync(term));

        // Gợi ý điểm đến
        [HttpGet("end-points")]
        [AllowAnonymous]
        public async Task<IActionResult> GetEndPoints(string term) =>
            Ok(await _trainService.GetEndPointsAsync(term));
    }
}
