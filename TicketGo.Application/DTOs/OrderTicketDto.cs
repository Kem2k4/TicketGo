using TicketGo.Domain.Entities;

namespace TicketGo.Application.DTOs
{
    public class OrderTicketDto
    {
        public Train Train { get; set; }
        public int IdTrain { get; set; }
        public List<Seat> OccupiedSeats { get; set; }
        public string PointStart { get; set; }
        public string PointEnd { get; set; }
        public string DateStart { get; set; }
        public decimal Price { get; set; }
        public string VehicleType { get; set; }
    }
}