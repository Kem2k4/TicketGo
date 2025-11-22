using TicketGo.Application.DTOs;
using TicketGo.Domain.Entities;
using TicketGo.Domain.Interfaces;
using TicketGo.Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace TicketGo.Application.Services
{
    public class TrainService : ITrainService
    {
        private readonly ITrainRouteRepository _trainRouteRepository;
        private readonly ITrainRepository _trainRepository;

        public TrainService(ITrainRouteRepository trainRouteRepository, ITrainRepository trainRepository)
        {
            _trainRouteRepository = trainRouteRepository;
            _trainRepository = trainRepository;
        }

        public async Task<List<string>> GetStartPointsAsync(string term)
        {
            return await _trainRouteRepository.GetStartPointsAsync(term);
        }

        public async Task<List<string>> GetEndPointsAsync(string term)
        {
            return await _trainRouteRepository.GetEndPointsAsync(term);
        }

        public async Task<PagedResult<TrainResponseDto>> SearchTrainsAsync(TrainSearchRequest request)
        {
            // Gọi repository với các tham số riêng lẻ
            var pagedTrains = await _trainRepository.SearchTrainsAsync(
                noiDi: request.NoiDi,
                noiDen: request.NoiDen,
                ngayKhoiHanh: request.NgayKhoiHanh,
                sortTime: request.SortTime,
                sortPrice: request.SortPrice,
                loaiXe: request.LoaiXe ?? new List<string>(),
                page: request.Page,
                pageSize: request.PageSize);

            // Normalize filter values để match với category trong DB (giống như trong repository)
            var normalizedLoaiXe = request.LoaiXe?.Select(f => 
            {
                var lower = f?.ToLower().Trim() ?? "";
                return lower switch
                {
                    "ghe" => "Ghế ngồi",
                    "ghế ngồi" => "Ghế ngồi",
                    "giuong" => "Giường nằm",
                    "giường nằm" => "Giường nằm",
                    "limousine" => "Limousine",
                    _ => f?.Trim() ?? "" // Giữ nguyên nếu không match
                };
            }).Where(nf => !string.IsNullOrEmpty(nf)).Distinct().ToList() ?? new List<string>();

            // Ánh xạ Train sang TrainResponseDto
            // Repository đã filter trains có coach phù hợp, nên ta chỉ cần map
            var trainDtos = pagedTrains.Items
                .Where(train => train.Coaches != null && train.Coaches.Any()) // Chỉ lấy train có ít nhất 1 coach
                .Select(train =>
                {
                    // Lấy coach phù hợp với filter (nếu có) hoặc coach đầu tiên
                    var coach = (request.LoaiXe != null && request.LoaiXe.Any() && normalizedLoaiXe.Any())
                        ? train.Coaches?.FirstOrDefault(c => c.Category != null && normalizedLoaiXe.Contains(c.Category))
                        : train.Coaches?.FirstOrDefault();
                    
                    // Nếu không tìm thấy coach phù hợp, bỏ qua train này
                    // (Điều này không nên xảy ra vì repository đã filter, nhưng để an toàn)
                    if (coach == null)
                        return null;
                    
                    return new TrainResponseDto
                    {
                        Id = train.IdTrain,
                        TenTau = train.NameTrain ?? "N/A",
                        NoiDi = train.IdTrainRouteNavigation?.PointStart ?? "N/A",
                        NoiDen = train.IdTrainRouteNavigation?.PointEnd ?? "N/A",
                        GioKhoiHanh = train.DateStart.HasValue ? train.DateStart.Value : default(DateTime),
                        GiaVe = coach.BasicPrice.HasValue ? (decimal?)coach.BasicPrice.Value : null,
                        LoaiXe = coach.Category ?? "N/A",
                        CoachID = coach.IdCoach
                    };
                })
                .Where(dto => dto != null)
                .ToList()!;

            // Vấn đề: Repository filter trains có coach phù hợp, nhưng service lại filter thêm khi map
            // Điều này có thể dẫn đến số items thực tế ít hơn TotalRecords
            // Giải pháp: Nếu có filter và số items ít hơn pageSize, tính lại TotalRecords
            // Nếu số items bằng pageSize, có thể còn nhiều records hơn, cần query lại để đếm chính xác
            var actualItemsCount = trainDtos.Count;
            int finalTotalRecords;
            
            if (request.LoaiXe != null && request.LoaiXe.Any() && actualItemsCount < pagedTrains.PageSize)
            {
                // Nếu số items ít hơn pageSize, đây là trang cuối hoặc chỉ có ít records
                // Tính TotalRecords = (page - 1) * pageSize + actualItemsCount
                finalTotalRecords = (pagedTrains.Page - 1) * pagedTrains.PageSize + actualItemsCount;
            }
            else if (request.LoaiXe != null && request.LoaiXe.Any() && actualItemsCount == 0)
            {
                // Nếu không có items nào, có thể là trang không hợp lệ hoặc filter quá strict
                // Giữ nguyên TotalRecords từ repository nhưng đảm bảo không nhỏ hơn số items đã skip
                finalTotalRecords = Math.Max(pagedTrains.TotalRecords, (pagedTrains.Page - 1) * pagedTrains.PageSize);
            }
            else
            {
                // Không có filter hoặc số items bằng pageSize (có thể còn nhiều records hơn)
                finalTotalRecords = pagedTrains.TotalRecords;
            }

            // Trả về kết quả phân trang
            return new PagedResult<TrainResponseDto>
            {
                Items = trainDtos,
                Page = pagedTrains.Page,
                PageSize = pagedTrains.PageSize,
                TotalRecords = finalTotalRecords
            };
        }

        public async Task<List<TrainDto>> GetAllTrainsAsync()
        {
            var trains = await _trainRepository.GetAllAsync();
            return trains.Select(t => new TrainDto
            {
                IdTrain = t.IdTrain,
                NameTrain = t.NameTrain,
                DateStart = t.DateStart,
                IdTrainRoute = t.IdTrainRoute,
                TrainRouteName = t.IdTrainRouteNavigation != null
                    ? $"{t.IdTrainRouteNavigation.PointStart} - {t.IdTrainRouteNavigation.PointEnd}"
                    : "N/A",
                CoefficientTrain = (double?)t.CoefficientTrain
            }).ToList();
        }

        public async Task<TrainDto> GetTrainByIdAsync(int id)
        {
            var train = await _trainRepository.GetByIdAsync(id);
            if (train == null)
            {
                return null;
            }

            return new TrainDto
            {
                IdTrain = train.IdTrain,
                NameTrain = train.NameTrain,
                DateStart = train.DateStart,
                IdTrainRoute = train.IdTrainRoute,
                TrainRouteName = train.IdTrainRouteNavigation != null
                    ? $"{train.IdTrainRouteNavigation.PointStart} - {train.IdTrainRouteNavigation.PointEnd}"
                    : "N/A",
                CoefficientTrain = (double?)train.CoefficientTrain
            };
        }

        public async Task CreateTrainAsync(TrainDto trainDto)
        {
            var train = new Train
            {
                NameTrain = trainDto.NameTrain,
                DateStart = trainDto.DateStart,
                IdTrainRoute = trainDto.IdTrainRoute,
                CoefficientTrain = (decimal?)trainDto.CoefficientTrain
            };

            await _trainRepository.AddAsync(train);
        }

        public async Task UpdateTrainAsync(int id, TrainDto trainDto)
        {
            var train = await _trainRepository.GetByIdAsync(id);
            if (train == null)
            {
                throw new Exception("Train not found");
            }

            train.NameTrain = trainDto.NameTrain;
            train.DateStart = trainDto.DateStart;
            train.IdTrainRoute = trainDto.IdTrainRoute;
            train.CoefficientTrain = (decimal?)trainDto.CoefficientTrain;

            await _trainRepository.UpdateAsync(train);
        }

        public async Task DeleteTrainAsync(int id)
        {
            await _trainRepository.DeleteAsync(id);
        }

        public async Task<List<TrainRouteDto>> GetAllTrainRoutesAsync()
        {
            var trainRoutes = await _trainRouteRepository.GetAllAsync();
            return trainRoutes.Select(tr => new TrainRouteDto
            {
                IdTrainRoute = tr.IdTrainRoute,
                PointStart = tr.PointStart,
                PointEnd = tr.PointEnd
            }).ToList();
        }
    }
}