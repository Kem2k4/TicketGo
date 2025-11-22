namespace TicketGo.Application.DTOs
{
    //[ Thêm tuyến đường && Cập nhật tuyến đường]
    public class TrainRouteDto
    {
        public int? IdTrainRoute { get; set; } = null;
        public string PointStart { get; set; }
        public string PointEnd { get; set; }
    }

}