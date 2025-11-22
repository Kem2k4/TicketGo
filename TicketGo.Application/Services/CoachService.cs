using TicketGo.Application.DTOs;
using TicketGo.Domain.Entities;
using TicketGo.Domain.Interfaces;
using TicketGo.Application.Interfaces;

namespace TicketGo.Application.Services
{
    public class CoachService : ICoachService
    {
        private readonly ICoachRepository _coachRepository;
        private readonly ITrainRepository _trainRepository;

        private readonly ISeatService _seatService;

        private readonly IUnitOfWork _unitOfWork;

        public CoachService(ICoachRepository coachRepository, ITrainRepository trainRepository, ISeatService seatService, IUnitOfWork unitOfWork)
        {
            _coachRepository = coachRepository;
            _trainRepository = trainRepository;
            _seatService = seatService;
            _unitOfWork = unitOfWork;
        }
        //[Danh sách xe]
        public async Task<List<CoachDto>> GetAllCoachesAsync()
        {
            var coaches = await _coachRepository.GetAllAsync();
            return coaches.Select(c => new CoachDto
            {
                IdCoach = c.IdCoach,
                NameCoach = c.NameCoach,
                Category = c.Category,
                SeatsQuantity = c.SeatsQuantity.Value,
                BasicPrice = (decimal)c.BasicPrice,
                IdTrain = c.IdTrain,
                TrainName = c.IdTrainNavigation?.IdTrain.ToString() // Có thể thay bằng thuộc tính phù hợp của Train
            }).ToList();
        }
        //[Chi tiết xe]
        public async Task<CoachDto> GetCoachByIdAsync(int id)
        {
            var coach = await _coachRepository.GetByIdAsync(id);
            if (coach == null)
            {
                return null;
            }

            return new CoachDto
            {
                IdCoach = coach.IdCoach,
                NameCoach = coach.NameCoach,
                Category = coach.Category,
                SeatsQuantity = coach.SeatsQuantity.Value,
                BasicPrice = (decimal)coach.BasicPrice,
                IdTrain = coach.IdTrain,
                TrainName = coach.IdTrainNavigation?.IdTrain.ToString() // Có thể thay bằng thuộc tính phù hợp của Train
            };
        }
        //[Thêm xe]
        public async Task CreateCoachAsync(CoachDto coachDto)
        {
            using var transaction = await _unitOfWork.BeginTransactionAsync();
            try
            {
                var coach = new Coach
                {
                    NameCoach = coachDto.NameCoach,
                    Category = coachDto.Category,
                    SeatsQuantity = coachDto.SeatsQuantity,
                    BasicPrice = (double)coachDto.BasicPrice,
                    IdTrain = coachDto.IdTrain
                };

                await _coachRepository.AddAsync(coach);
                await _unitOfWork.SaveChangesAsync();

                await _seatService.CreateSeatAsync(coach.IdCoach, coach.SeatsQuantity.Value);
                await _unitOfWork.SaveChangesAsync();

                await _unitOfWork.CommitAsync();
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }

        public async Task UpdateCoachAsync(int id, CoachDto coachDto)
        {
            var coach = await _coachRepository.GetByIdAsync(id);
            if (coach == null)
            {
                throw new Exception("Coach not found");
            }

            coach.NameCoach = coachDto.NameCoach;
            coach.Category = coachDto.Category;
            coach.SeatsQuantity = coachDto.SeatsQuantity;
            coach.BasicPrice = (double?)coachDto.BasicPrice;
            coach.IdTrain = coachDto.IdTrain;

            await _coachRepository.UpdateAsync(coach);
        }

        public async Task DeleteCoachAsync(int id)
        {
            await _coachRepository.DeleteAsync(id);
        }

       public async Task<List<TrainDto>> GetAllTrainsAsync()
        {
            var trains = await _trainRepository.GetAllAsync();
            return trains.Select(t => new TrainDto
            {
                IdTrain = t.IdTrain,
                NameTrain = t.NameTrain
            }).ToList();
        }
    }
}