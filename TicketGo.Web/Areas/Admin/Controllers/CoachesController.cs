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
    public class CoachesController : Controller
    {
        private readonly ICoachService _coachService;
        private readonly ITrainService _trainService; 

        private readonly ISeatService _seatService;

        public CoachesController(ICoachService coachService, ITrainService trainService, ISeatService seatService)
        {
            _seatService = seatService;
            _coachService = coachService;
            _trainService = trainService; 
        }

        //[Danh sách xe]
        // GET: Admin/Coaches
        public async Task<IActionResult> Index()
        {
            var coaches = await _coachService.GetAllCoachesAsync();
            var trains = await _trainService.GetAllTrainsAsync(); 
            ViewData["Trains"] = trains; 
            return View(coaches);
        }
        //[Thêm xe]
        // POST: Admin/Coaches/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] CoachDto coachDto)
        {
            // Server-side validation
            if (string.IsNullOrWhiteSpace(coachDto.NameCoach))
            {
                ModelState.AddModelError("NameCoach", "Tên xe không được để trống");
            }
            else if (!coachDto.NameCoach.All(c => char.IsLetterOrDigit(c) || char.IsWhiteSpace(c)))
            {
                ModelState.AddModelError("NameCoach", "Tên xe chỉ được chứa chữ cái, số và khoảng trắng");
            }
            if (string.IsNullOrWhiteSpace(coachDto.Category))
            {
                ModelState.AddModelError("Category", "Loại xe không được để trống");
            }
            else if (!coachDto.Category.All(c => char.IsLetter(c) || char.IsWhiteSpace(c)))
            {
                ModelState.AddModelError("Category", "Loại xe chỉ được chứa chữ cái và khoảng trắng");
            }
            if (coachDto.SeatsQuantity < 1)
            {
                ModelState.AddModelError("SeatsQuantity", "Số lượng ghế phải lớn hơn 0");
            }
            if (coachDto.BasicPrice < 0)
            {
                ModelState.AddModelError("BasicPrice", "Giá cơ bản không được âm");
            }

            if (!ModelState.IsValid)
            {
                return RedirectToAction(nameof(Index));
            }

            try
            {
                await _coachService.CreateCoachAsync(coachDto);

                    
                var updatedCoaches = await _coachService.GetAllCoachesAsync();
                var trains = await _trainService.GetAllTrainsAsync();
                // Re-pass trains on error
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Lỗi khi tạo xe: " + ex.Message);
                var coaches = await _coachService.GetAllCoachesAsync();
                var trains = await _trainService.GetAllTrainsAsync();
                // Re-pass trains on error
                return RedirectToAction(nameof(Index));
            }
        }
        //[Sửa thông tin xe]
        // POST: Admin/Coaches/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromForm] CoachDto coachDto)
        {

            // Server-side validation
            if (string.IsNullOrWhiteSpace(coachDto.NameCoach))
            {
                ModelState.AddModelError("NameCoach", "Tên xe không được để trống");
            }
            else if (!coachDto.NameCoach.All(c => char.IsLetterOrDigit(c) || char.IsWhiteSpace(c)))
            {
                ModelState.AddModelError("NameCoach", "Tên xe chỉ được chứa chữ cái, số và khoảng trắng");
            }
            if (string.IsNullOrWhiteSpace(coachDto.Category))
            {
                ModelState.AddModelError("Category", "Loại xe không được để trống");
            }
            else if (!coachDto.Category.All(c => char.IsLetter(c) || char.IsWhiteSpace(c)))
            {
                ModelState.AddModelError("Category", "Loại xe chỉ được chứa chữ cái và khoảng trắng");
            }
            if (coachDto.SeatsQuantity < 1)
            {
                ModelState.AddModelError("SeatsQuantity", "Số lượng ghế phải lớn hơn 0");
            }
            if (coachDto.BasicPrice < 0)
            {
                ModelState.AddModelError("BasicPrice", "Giá cơ bản không được âm");
            }

            if (!ModelState.IsValid)
            {   
                TempData["ErrorMessages"] = "Error: Dữ liệu không hợp lệ!";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                var existingCoach = await _coachService.GetCoachByIdAsync(coachDto.IdCoach.Value);
                if (existingCoach == null)
                {
                    return NotFound();
                }

                await _coachService.UpdateCoachAsync(coachDto.IdCoach.Value, coachDto);
                TempData["SuccessMessage"] = "Xe đã được cập nhật!";
                var updatedCoaches = await _coachService.GetAllCoachesAsync();
                var trains = await _trainService.GetAllTrainsAsync();
                // Re-pass trains on error
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Lỗi khi cập nhật xe: " + ex.Message);
                var coaches = await _coachService.GetAllCoachesAsync();
                var trains = await _trainService.GetAllTrainsAsync();
                // Re-pass trains on error
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Admin/Coaches/Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {

            var coach = await _coachService.GetCoachByIdAsync(id);
            if (coach == null)
            {
                return NotFound();
            }

            try 
            {
                // Xóa ghế trong xe
                await _seatService.DeleteSeatsAsync(coach.IdCoach.Value);
                // Xóa xe
                await _coachService.DeleteCoachAsync(coach.IdCoach.Value);
                TempData["SuccessMessage"] = "Xe đã được xóa!";

            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Lỗi khi xóa xe: " + ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }
    }
}