using TicketGo.Domain.Entities;
using TicketGo.Application.DTOs;

namespace TicketGo.Application.Interfaces
{
    public interface ITrainRouteService
    {
        // [Lấy danh sách tất cả tuyến đường]
        Task<List<TrainRouteDto>> GetAllTrainRoutesAsync();
        // [Lấy thông tin chi tiết tuyến đường]
        Task<TrainRouteDto> GetTrainRouteByIdAsync(int id);
        // [Thêm tuyến đường]
        Task CreateTrainRouteAsync(TrainRouteDto trainRouteDto);
        // [Cập nhật thông tin tuyến đường]
        Task UpdateTrainRouteAsync(int id, TrainRouteDto trainRouteDto);
        // [Xóa tuyến đường]
        Task DeleteTrainRouteAsync(int id);
    }
}