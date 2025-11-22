using TicketGo.Application.DTOs;
using TicketGo.Domain.Entities;
using TicketGo.Domain.Interfaces;
using TicketGo.Application.Interfaces;

namespace TicketGo.Application.Services
{
    public class TrainRouteService : ITrainRouteService
    {
        private readonly ITrainRouteRepository _trainRouteRepository;

        public TrainRouteService(ITrainRouteRepository trainRouteRepository)
        {
            _trainRouteRepository = trainRouteRepository;
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

        public async Task<TrainRouteDto> GetTrainRouteByIdAsync(int id)
        {
            var trainRoute = await _trainRouteRepository.GetByIdAsync(id);
            if (trainRoute == null)
            {
                return null;
            }

            return new TrainRouteDto
            {
                IdTrainRoute = trainRoute.IdTrainRoute,
                PointStart = trainRoute.PointStart,
                PointEnd = trainRoute.PointEnd
            };
        }

        public async Task CreateTrainRouteAsync(TrainRouteDto trainRouteDto)
        {
            var trainRoute = new TrainRoute
            {
                PointStart = trainRouteDto.PointStart,
                PointEnd = trainRouteDto.PointEnd
            };

            await _trainRouteRepository.AddAsync(trainRoute);
        }

        public async Task UpdateTrainRouteAsync(int id, TrainRouteDto trainRouteDto)
        {
            var trainRoute = await _trainRouteRepository.GetByIdAsync(id);
            if (trainRoute == null)
            {
                throw new Exception("TrainRoute not found");
            }

            trainRoute.PointStart = trainRouteDto.PointStart;
            trainRoute.PointEnd = trainRouteDto.PointEnd;

            await _trainRouteRepository.UpdateAsync(trainRoute);
        }

        public async Task DeleteTrainRouteAsync(int id)
        {
            await _trainRouteRepository.DeleteAsync(id);
        }
    }
}