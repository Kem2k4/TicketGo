namespace TicketGo.Application.DTOs
{

    public class OrderDto
    {
        public int IdOrder { get; set; }
        public double? TotalPrice { get; set; }
        public DateTime? DateOrder { get; set; }

        public string? DiscountName { get; set; } // Thêm thuộc tính này để lưu tên giảm giá
        public int? IdDiscount { get; set; }
        public string NameCus { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public int? IdAccount { get; set; }
        public int? IdCoach { get; set; } // Thêm để lưu IdCoach
        public List<string> ListSeats { get; set; } = new List<string>(); // Danh sách ghế đã đặt

        // Thông tin vé bổ sung cho phần quản lý đơn hàng
        public string? TrainName { get; set; }
        public string? PointStart { get; set; }
        public string? PointEnd { get; set; }
        public DateTime? DepartureTime { get; set; }
        public string? CoachName { get; set; }
        public string? VehicleType { get; set; }
    }
}