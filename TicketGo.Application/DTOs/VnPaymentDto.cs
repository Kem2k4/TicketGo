namespace TicketGo.Application.DTOs
{
    public class VnPaymentResponseDto
    {
        public bool Success { get; set; }
        public string PaymentMethod { get; set; }= null!;
        public string OrderDescription { get; set; }= null!;
        public string OrderId { get; set; }
        public string PaymentId { get; set; }
        public string TransactionId { get; set; }= null!;
        public string Token { get; set; }= null!;
        public string VnPayResponseCode { get; set; }= null!;
    }
    public class VnPayRequestDto 
    {
        public int OrderId { get; set; }
        public string Fullname { get; set; }
        public string Description {  get; set; }= null!;
        public double Amount { get; set; }
        public DateTime CreatedDate { get; set; }
    }

}
