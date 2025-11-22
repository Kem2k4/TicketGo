using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TicketGo.Application.Interfaces;
using TicketGo.Application.DTOs;
using X.PagedList;
using Microsoft.AspNetCore.Authorization;

namespace TicketGo.Web.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    public class TicketController : Controller
    {
        private readonly ITicketService _ticketService;

        public TicketController(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> DanhMucVe(int? page)
        {
            int pageSize = 8;
            int pageNumber = page == null || page < 0 ? 1 : page.Value;

            var lstVe = await _ticketService.GetPagedTicketsAsync(pageNumber, pageSize);
            return View(lstVe);
        }

        public async Task<IActionResult> SuaVe(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = await _ticketService.GetTicketByIdAsync(id.Value);
            if (ticket == null)
            {
                return NotFound();
            }

            var seats = await _ticketService.GetAllSeatsAsync();
            var trains = await _ticketService.GetAllTrainsAsync();

            ViewData["IdSeat"] = new SelectList(seats, "IdSeat", "NameSeat", ticket.IdSeat);
            ViewData["IdTrain"] = new SelectList(trains, "IdTrain", "TrainName", ticket.IdTrain);

            var ticketDto = new CreateUpdateTicketDto
            {
                IdTicket = ticket.IdTicket,
                Date = ticket.Date,
                Price = ticket.Price,
                IdSeat = ticket.IdSeat,
                IdTrain = ticket.IdTrain
            };

            return View(ticketDto);
        }


        public async Task<IActionResult> SuaVe(int id, CreateUpdateTicketDto ticketDto)
        {
            if (id != ticketDto.IdTicket)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _ticketService.UpdateTicketAsync(id, ticketDto);
                }
                catch (Exception)
                {
                    if (await _ticketService.GetTicketByIdAsync(id) != null)
                    {
                        return NotFound();
                    }
                    throw;
                }
                return RedirectToAction(nameof(DanhMucVe));
            }

            var seats = await _ticketService.GetAllSeatsAsync();
            var trains = await _ticketService.GetAllTrainsAsync();

            ViewData["IdSeat"] = new SelectList(seats, "IdSeat", "NameSeat", ticketDto.IdSeat);
            ViewData["IdTrain"] = new SelectList(trains, "IdTrain", "TrainName", ticketDto.IdTrain);

            return View(ticketDto);
        }

        [HttpGet]
        public async Task<IActionResult> XoaVe(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = await _ticketService.GetTicketByIdAsync(id.Value);
            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        [HttpPost]
        public async Task<IActionResult> XacNhanXoa(int id)
        {
            await _ticketService.DeleteTicketAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}