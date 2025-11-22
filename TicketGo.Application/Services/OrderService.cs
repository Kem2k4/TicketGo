using TicketGo.Application.DTOs;
using TicketGo.Domain.Entities;
using TicketGo.Domain.Interfaces;
using TicketGo.Application.Interfaces;
using System.Linq;

namespace TicketGo.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IDiscountRepository _discountRepository;
        private readonly ICoachRepository _coachRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly ISeatRepository _seatRepository;
        private readonly ITicketRepository _ticketRepository;
        private readonly IOrderTicketRepository _orderTicketRepository;
        private readonly ITrainRepository _trainRepository;
        private readonly IAccountRepository _accountRepository;

        public OrderService(
            ICoachRepository coachRepository,
            IOrderRepository orderRepository,
            ISeatRepository seatRepository,
            ITicketRepository ticketRepository,
            IOrderTicketRepository orderTicketRepository,
            IDiscountRepository discountRepository,
            ITrainRepository trainRepository,
            IAccountRepository accountRepository)
        {
            _coachRepository = coachRepository;
            _orderRepository = orderRepository;
            _seatRepository = seatRepository;
            _ticketRepository = ticketRepository;
            _orderTicketRepository = orderTicketRepository;
            _discountRepository = discountRepository;
            _trainRepository = trainRepository;
            _accountRepository = accountRepository;
        }

        public async Task<List<OrderDto>> GetAllOrdersAsync()
        {
            var orders = await _orderRepository.GetAllAsync();
            return orders.Select(MapToOrderDto).ToList();
        }

        public async Task<OrderDto> GetOrderByIdAsync(int id)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            if (order == null)
            {
                return null;
            }

            return MapToOrderDto(order);
        }

        public async Task<OrderTicketDto> GetOrderTicketDetailsAsync(int idCoach)
        {
            var coach = await _coachRepository.GetCoachWithRelatedDataAsync(idCoach);
            if (coach == null)
            {
                return null;
            }

            var occupiedSeats = coach.Seats.Where(s => s.State).ToList();
            var coachCategory = coach.Category;
            var ticketPrice = CalculateTicketPrice(coach.IdTrainNavigation);

            return CreateOrderTicketDto(coach, occupiedSeats, coachCategory, ticketPrice);
        }

        public async Task CreateOrderAsync(OrderDto orderDto)
        {
            var order = new Order
            {
                UnitPrice = orderDto.TotalPrice,
                DateOrder = orderDto.DateOrder ?? DateTime.Now,
                NameCus = orderDto.NameCus,
                Phone = orderDto.Phone,
                IdCus = orderDto.IdAccount,
                IdDiscount = orderDto.IdDiscount ?? null
            };

            await _orderRepository.AddAsync(order);

            foreach (var seatName in orderDto.ListSeats)
            {
                if (orderDto.IdCoach == null)
                    throw new ArgumentException("IdCoach is required");

                var seat = await _seatRepository.GetByNameAndCoachIdAsync(seatName, orderDto.IdCoach.Value);
                if (seat == null)
                    throw new InvalidOperationException($"Không tìm thấy ghế {seatName} cho xe {orderDto.IdCoach}");

                if (!seat.IdSeat.HasValue)
                    throw new InvalidOperationException($"Ghế {seatName} không có định danh hợp lệ.");

                seat.State = true;
                await _seatRepository.UpdateAsync(seat);

                var coach = await _coachRepository.GetCoachWithRelatedDataAsync(seat.IdCoach)
                    ?? throw new InvalidOperationException($"Không tìm thấy thông tin xe {seat.IdCoach}.");

                if (coach.IdTrain == null || coach.IdTrainNavigation == null)
                    throw new InvalidOperationException("Xe chưa được gán chuyến, không thể tạo vé.");

                var ticket = new Ticket
                {
                    Date = DateTime.Now,
                    Price = orderDto.TotalPrice ?? 0,
                    IdSeat = seat.IdSeat.Value,
                    IdTrain = coach.IdTrain.Value
                };

                await _ticketRepository.AddAsync(ticket);

                var orderTicket = new OrderTicket
                {
                    IdOrder = order.IdOrder,
                    IdTicket = ticket.IdTicket
                };

                await _orderTicketRepository.AddAsync(orderTicket);
            }
        }

        public async Task<List<OrderDto>> GetOrdersByAccountAsync(int accountId)
        {
            var orders = await _orderRepository.GetAllAsync();
            return orders
                .Where(o => o.IdCus == accountId)
                .Select(MapToOrderDto)
                .ToList();
        }

        private OrderDto MapToOrderDto(Order order)
        {
            var tickets = order.OrderTickets?
                .Select(ot => ot.IdTicketNavigation)
                .Where(t => t != null)
                .ToList() ?? new List<Ticket>();

            var firstTicket = tickets.FirstOrDefault();
            var train = firstTicket?.IdTrainNavigation;
            var coach = firstTicket?.IdSeatNavigation?.IdCoachNavigation;
            var seatNames = tickets
                .Select(t => t?.IdSeatNavigation?.NameSeat)
                .Where(name => !string.IsNullOrWhiteSpace(name))
                .Select(name => name!)
                .ToList();

            return new OrderDto
            {
                IdOrder = order.IdOrder,
                TotalPrice = order.UnitPrice,
                DateOrder = order.DateOrder,
                IdDiscount = order.IdDiscount,
                DiscountName = order.IdDiscountNavigation?.NameDiscount,
                NameCus = order.NameCus ?? string.Empty,
                Phone = order.Phone ?? string.Empty,
                IdAccount = order.IdAccountNavigation?.IdAccount ?? order.IdCus,
                ListSeats = seatNames,
                TrainName = train?.NameTrain,
                PointStart = train?.IdTrainRouteNavigation?.PointStart,
                PointEnd = train?.IdTrainRouteNavigation?.PointEnd,
                DepartureTime = train?.DateStart,
                CoachName = coach?.NameCoach,
                VehicleType = coach?.Category
            };
        }

        private decimal CalculateTicketPrice(Train train)
        {
            var basicPrice = (decimal)(train.Coaches.FirstOrDefault()?.BasicPrice ?? 0);
            return (decimal)(train.CoefficientTrain ?? 1) * basicPrice;
        }

        private OrderTicketDto CreateOrderTicketDto(Coach coach, List<Seat> occupiedSeats, string coachCategory, decimal ticketPrice)
        {
            var train = coach.IdTrainNavigation;
            return new OrderTicketDto
            {
                Train = train,
                IdTrain = train.IdTrain,
                OccupiedSeats = occupiedSeats,
                PointStart = train.IdTrainRouteNavigation.PointStart,
                PointEnd = train.IdTrainRouteNavigation.PointEnd,
                DateStart = train.DateStart?.ToShortDateString(),
                Price = ticketPrice,
                VehicleType = coachCategory
            };
        }

        public async Task UpdateOrderAsync(int id, OrderDto orderDto)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            if (order == null)
            {
                throw new Exception("Đơn hàng không tồn tại");
            }

            order.UnitPrice = orderDto.TotalPrice ?? 0;
            order.DateOrder = orderDto.DateOrder ?? DateTime.Now;
            order.IdDiscount = orderDto.IdDiscount ?? 0;
            order.NameCus = orderDto.NameCus;
            order.Phone = orderDto.Phone;
            order.IdCus = orderDto.IdAccount; // Sử dụng orderDto, không phải Coach

            await _orderRepository.UpdateAsync(order);
        }

        public async Task DeleteOrderAsync(int id)
        {
            await _orderRepository.DeleteAsync(id);
        }

        public async Task<DashboardStatsDto> GetDashboardStatsAsync()
        {
            var orders = await _orderRepository.GetAllAsync();
            var trains = await _trainRepository.GetAllAsync();
            var accounts = await _accountRepository.GetAllAsync();
            
            // Count tickets from orders
            var totalTickets = orders
                .SelectMany(o => o.OrderTickets ?? Enumerable.Empty<OrderTicket>())
                .Count();

            var totalOrders = orders.Count;
            var totalRevenue = orders
                .Where(o => o.UnitPrice.HasValue)
                .Sum(o => (decimal)o.UnitPrice.Value);
            var totalTrains = trains.Count;
            var totalAccounts = accounts.Count;

            // Revenue by month (last 6 months)
            var revenueByMonth = orders
                .Where(o => o.DateOrder.HasValue && o.UnitPrice.HasValue)
                .GroupBy(o => new { o.DateOrder.Value.Year, o.DateOrder.Value.Month })
                .OrderBy(g => g.Key.Year).ThenBy(g => g.Key.Month)
                .TakeLast(6)
                .Select(g => new RevenueByMonthDto
                {
                    Month = $"{g.Key.Month}/{g.Key.Year}",
                    Revenue = (decimal)g.Sum(o => o.UnitPrice!.Value)
                })
                .ToList();

            // Orders by status (simplified - all orders are considered completed)
            var ordersByStatus = new List<OrderStatusDto>
            {
                new OrderStatusDto { Status = "Hoàn thành", Count = totalOrders }
            };

            return new DashboardStatsDto
            {
                TotalOrders = totalOrders,
                TotalRevenue = totalRevenue,
                TotalTickets = totalTickets,
                TotalTrains = totalTrains,
                TotalAccounts = totalAccounts,
                RevenueByMonth = revenueByMonth,
                OrdersByStatus = ordersByStatus
            };
        }

    }
}