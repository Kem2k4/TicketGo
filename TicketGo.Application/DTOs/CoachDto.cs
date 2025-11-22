namespace TicketGo.Application.DTOs
{
    //[Danh sách xe, thêm xe, sửa thông tin xe]
    public class CoachDto
    {
        public int? IdCoach { get; set; }
        public string NameCoach { get; set; }
        public string Category { get; set; }
        public int SeatsQuantity { get; set; }
        public decimal BasicPrice { get; set; }
        public int? IdTrain { get; set; } = null;
        public string? TrainName { get; set; } = null; // Tên tàu (từ IdTrainNavigation)
    }
}