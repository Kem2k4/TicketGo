namespace TicketGo.Application.DTOs
{
    public class TrainDto
        {
            public int? IdTrain { get; set; }
            public string NameTrain { get; set; } = null!;
            public DateTime? DateStart { get; set; }
            public int IdTrainRoute { get; set; }
            public string? TrainRouteName { get; set; } // Tên tuyến tàu (từ IdTrainRouteNavigation)
            public double? CoefficientTrain { get; set; }
        }

    public class TrainSearchRequest : PagingRequest
        {
            public string? NoiDi { get; set; }
            public string? NoiDen { get; set; }
            public DateTime? NgayKhoiHanh { get; set; }
            public string? SortTime { get; set; } // ví dụ: "time:asc", "time:desc"
            public string? SortPrice { get; set; } // ví dụ: "fare:asc", "fare:desc"
            public List<string> LoaiXe { get; set; } = new List<string>();
        }

    
    public class TrainResponseDto
        {
            public int Id { get; set; }
            public string? TenTau { get; set; }
            public string? NoiDi { get; set; }
            public string? NoiDen { get; set; }
            public DateTime GioKhoiHanh { get; set; }
            public decimal? GiaVe { get; set; }
            public string? LoaiXe { get; set; }
            public int CoachID { get; set; } 
        }

}