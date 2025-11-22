namespace TicketGo.Application.DTOs
{
    public class CreateOrderDto
    {
        public List<string> ListSeats { get; set; }
        public string Fullname { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public decimal TotalPrice { get; set; }
        public int IdCoach { get; set; }
        public int IdCustomer { get; set; }
    }
}