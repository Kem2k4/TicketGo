using TicketGo.Application.DTOs;

namespace TicketGo.Application.Interfaces
{
    public interface ITrainService
    {
        //[Tìm kiếm điểm khởi hành]
        Task<List<string>> GetStartPointsAsync(string term);
        //[Tìm kiếm điểm đến]
        Task<List<string>> GetEndPointsAsync(string term);
        //[Tìm kiếm chuyến xe]
        Task<PagedResult<TrainResponseDto>> SearchTrainsAsync(TrainSearchRequest searchDto);
        //[Lấy danh sách tất cả các chuyến xe]
        Task<List<TrainDto>> GetAllTrainsAsync();


        
        Task<TrainDto> GetTrainByIdAsync(int id);
        Task CreateTrainAsync(TrainDto trainDto);
        Task UpdateTrainAsync(int id, TrainDto trainDto);
        Task DeleteTrainAsync(int id);
        Task<List<TrainRouteDto>> GetAllTrainRoutesAsync();
    }
}