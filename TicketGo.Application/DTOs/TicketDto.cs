namespace TicketGo.Application.DTOs
{
    public class TicketDto
    {
        public int IdTicket { get; set; }
        public DateTime? Date { get; set; }
        public double? Price { get; set; }
        public int IdSeat { get; set; }
        public string SeatName { get; set; } // Tên ghế (từ IdSeatNavigation)
        public int IdTrain { get; set; }
        public string TrainName { get; set; } // Tên tàu (từ IdTrainNavigation)
    }

    public class CreateUpdateTicketDto
    {
        public int IdTicket { get; set; }
        public DateTime? Date { get; set; }
        public double? Price { get; set; }
        public int IdSeat { get; set; }
        public int IdTrain { get; set; }
    }
}