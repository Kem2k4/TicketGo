using TicketGo.Application.DTOs;
using TicketGo.Domain.Entities;
using TicketGo.Domain.Interfaces;
using X.PagedList;
using TicketGo.Application.Interfaces;

namespace TicketGo.Application.Services
{
    public class TicketService : ITicketService
    {
        private readonly ITicketRepository _ticketRepository;
        private readonly ISeatRepository _seatRepository;
        private readonly ITrainRepository _trainRepository;

        public TicketService(ITicketRepository ticketRepository, ISeatRepository seatRepository, ITrainRepository trainRepository)
        {
            _ticketRepository = ticketRepository;
            _seatRepository = seatRepository;
            _trainRepository = trainRepository;
        }

        public async Task<IPagedList<TicketDto>> GetPagedTicketsAsync(int pageNumber, int pageSize)
        {
            var pagedTickets = await _ticketRepository.GetPagedTicketsAsync(pageNumber, pageSize);
            var ticketDtos = pagedTickets.Select(t => new TicketDto
            {
                IdTicket = t.IdTicket,
                Date = t.Date,
                Price = t.Price,
                IdSeat = t.IdSeat,
                SeatName = t.IdSeatNavigation?.NameSeat,
                IdTrain = t.IdTrain,
                TrainName = t.IdTrainNavigation?.IdTrain.ToString() // Có thể thay bằng thuộc tính phù hợp của Train
            }).ToList();

            return new StaticPagedList<TicketDto>(ticketDtos, pagedTickets.GetMetaData());
        }

        public async Task<TicketDto> GetTicketByIdAsync(int id)
        {
            var ticket = await _ticketRepository.GetByIdAsync(id);
            if (ticket == null)
            {
                return null;
            }

            return new TicketDto
            {
                IdTicket = ticket.IdTicket,
                Date = ticket.Date,
                Price = ticket.Price,
                IdSeat = ticket.IdSeat,
                SeatName = ticket.IdSeatNavigation?.NameSeat,
                IdTrain = ticket.IdTrain,
                TrainName = ticket.IdTrainNavigation?.IdTrain.ToString() // Có thể thay bằng thuộc tính phù hợp của Train
            };
        }

        public async Task UpdateTicketAsync(int id, CreateUpdateTicketDto ticketDto)
        {
            var ticket = await _ticketRepository.GetByIdAsync(id);
            if (ticket == null)
            {
                throw new Exception("Ticket not found");
            }

            ticket.Date = ticketDto.Date;
            ticket.Price = ticketDto.Price;
            ticket.IdSeat = ticketDto.IdSeat;
            ticket.IdTrain = ticketDto.IdTrain;

            await _ticketRepository.UpdateAsync(ticket);
        }

        public async Task DeleteTicketAsync(int id)
        {
            await _ticketRepository.DeleteAsync(id);
        }

        public async Task<List<SeatDto>> GetAllSeatsAsync()
        {
            var seats = await _seatRepository.GetAllAsync();
            return seats.Select(s => new SeatDto
            {
                IdSeat = s.IdSeat.Value,
                NameSeat = s.NameSeat
            }).ToList();
        }

        public async Task<List<TrainDto>> GetAllTrainsAsync()
        {
            var trains = await _trainRepository.GetAllAsync();
            return trains.Select(t => new TrainDto
            {
                IdTrain = t.IdTrain,
                NameTrain = t.NameTrain 
            }).ToList();
        }
    }
}