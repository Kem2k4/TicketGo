using Microsoft.AspNetCore.Mvc;
using TicketGo.Application.Interfaces;
using TicketGo.Application.DTOs;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Antiforgery;

namespace TicketGo.Web.Areas.Admin.Controllers
{   
    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    public class TrainRoutesController : Controller
    {
        private readonly ITrainRouteService _trainRouteService;

        public TrainRoutesController(ITrainRouteService trainRouteService)
        {
            _trainRouteService = trainRouteService;
        }

        //[Danh đường tuyến đường]
        // GET: Admin/TrainRoutes
        public async Task<IActionResult> Index()
        {
            var trainRoutes = await _trainRouteService.GetAllTrainRoutesAsync();
            return View(trainRoutes);
        }

        //[Thêm tuyến đường]
        // POST: Admin/TrainRoutes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] TrainRouteDto trainRouteDto)
        {

            // Server-side validation
            if (string.IsNullOrWhiteSpace(trainRouteDto.PointStart) || string.IsNullOrWhiteSpace(trainRouteDto.PointEnd))
            {
                ModelState.AddModelError("", "Điểm đi và điểm đến không được để trống");
            }
            else if (!trainRouteDto.PointStart.All(c => char.IsLetter(c) || char.IsWhiteSpace(c)) ||
                     !trainRouteDto.PointEnd.All(c => char.IsLetter(c) || char.IsWhiteSpace(c)))
            {
                ModelState.AddModelError("", "Điểm đi và điểm đến chỉ được chứa chữ cái và khoảng trắng");
            }
            else if (trainRouteDto.PointStart.Trim() == trainRouteDto.PointEnd.Trim())
            {
                ModelState.AddModelError("PointEnd", "Điểm đi và điểm đến không được trùng nhau");
            }

            if (!ModelState.IsValid)
            {
                return RedirectToAction(nameof(Index));
            }

            try
            {
                await _trainRouteService.CreateTrainRouteAsync(trainRouteDto);
                TempData["SuccessMessage"] = "Tuyến đường đã được thêm!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Lỗi khi tạo tuyến đường: " + ex.Message);
                return RedirectToAction(nameof(Index));
            }
        }

        //[Sửa tuyến đường]
        // POST: Admin/TrainRoutes/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromForm] TrainRouteDto trainRouteDto)
        {
            // Server-side validation
            if (string.IsNullOrWhiteSpace(trainRouteDto.PointStart) || string.IsNullOrWhiteSpace(trainRouteDto.PointEnd))
            {
                ModelState.AddModelError("", "Điểm đi và điểm đến không được để trống");
            }
            else if (!trainRouteDto.PointStart.All(c => char.IsLetter(c) || char.IsWhiteSpace(c)) ||
                     !trainRouteDto.PointEnd.All(c => char.IsLetter(c) || char.IsWhiteSpace(c)))
            {
                ModelState.AddModelError("", "Điểm đi và điểm đến chỉ được chứa chữ cái và khoảng trắng");
            }
            else if (trainRouteDto.PointStart.Trim() == trainRouteDto.PointEnd.Trim())
            {
                ModelState.AddModelError("PointEnd", "Điểm đi và điểm đến không được trùng nhau");
            }

            if (!ModelState.IsValid)
            {
                return RedirectToAction(nameof(Index));
            }

            try
            {
                var existingRoute = await _trainRouteService.GetTrainRouteByIdAsync(trainRouteDto.IdTrainRoute.Value);
                if (existingRoute == null) return NotFound();

                await _trainRouteService.UpdateTrainRouteAsync(trainRouteDto.IdTrainRoute.Value, trainRouteDto);
                TempData["SuccessMessage"] = "Tuyến đường đã được cập nhật!";

                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "Đã xảy ra lỗi khi cập nhật tuyến đường.";
                return RedirectToAction(nameof(Index));
            }
        }

        // [Xóa tuyến đường]
        // POST: Admin/TrainRoutes/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var trainRoute = await _trainRouteService.GetTrainRouteByIdAsync(id);
            if (trainRoute == null)
            {
                return NotFound();
            }
            try
            {
                await _trainRouteService.DeleteTrainRouteAsync(id);
                TempData["SuccessMessage"] = "Đã xóa tuyến đường!";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Lỗi khi xóa tuyến đường: " + ex.Message;
            }
            return RedirectToAction(nameof(Index));
        }
    }
}