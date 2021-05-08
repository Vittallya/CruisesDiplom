using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DAL;
using DAL.Dto;
using DAL.Models;
using AutoMapper;
using System.Data.Entity;

namespace BL
{
    public class OrderService
    {
        private readonly AllDbContext dbContext;
        private readonly MapperService mapper;
        private readonly InsurancesService insurancesService;
        private Order _currentOrder;
        private int _clientId;
        private int _tourId;
        public bool IsEdit { get; private set; }
        public bool IsStarted { get; private set; }
        public void Clear()
        {
            IsEdit = false;
            IsStarted = false;
            _currentOrder = null;
            _insurancesDto = null;
            _placementsDto = null;
        }
        public OrderService(AllDbContext dbContext, MapperService mapperService, InsurancesService insurancesService)
        {
            this.dbContext = dbContext;
            this.mapper = mapperService;
            this.insurancesService = insurancesService;
        }
        public void SetupClient(int clientId)
        {
            _clientId = clientId;
        }
        public async Task StartEditOrder(int orderId)
        {
            IsEdit = true;

            await dbContext.Orders.LoadAsync();
            _currentOrder = await dbContext.Orders.FindAsync(orderId);
        }
        public void SetupTour(int tourId)
        {
            _tourId = tourId;
        }
        public OrderDto GetOrder()
        {
            return _currentOrder != null ? mapper.MapTo<Order, OrderDto>(_currentOrder) : 
                new OrderDto() {  OrderStatus = OrderStatus.Active};
        }
        public int TourId => _tourId;
        public void SetupFilledOrder(OrderDto order)
        {
            _currentOrder = mapper.MapTo<OrderDto, Order>(order);
        }
        private IEnumerable<InsuranceDto> _insurancesDto;
        private IEnumerable<PlacementDto> _placementsDto;
        public bool HasInsurances => _insurancesDto != null && _insurancesDto.Count() > 0;
        public void SetupInsurances(IEnumerable<InsuranceDto> ins)
        {
            _insurancesDto = ins;
        }
        public void SetupPlacements(IEnumerable<PlacementDto> placements)
        {
            _placementsDto = placements;
        }
        public async Task<bool> ApplyOrder()
        {
            await dbContext.Tours.LoadAsync();

            
            _currentOrder.TourId = _tourId;

            if (!IsEdit)
            {
                _currentOrder.CreationDate = DateTimeOffset.Now;
                _currentOrder.OrderStatus = OrderStatus.Active;
                _currentOrder.ClientId = _clientId;

                

                if (HasInsurances)
                {
                    var insIds = _insurancesDto.Select(x => x.Id);

                    foreach (var ins in insIds)
                    {
                        var i = await dbContext.Insurances.FindAsync(ins);
                        i.Orders = new List<Order>();
                        i.Orders.Add(_currentOrder);
                    }
                }
                var placements = _placementsDto.Select(x =>
                {
                    var inst = mapper.MapTo<PlacementDto, Placement>(x);
                    inst.Order = _currentOrder;
                    return inst;
                });

                dbContext.Orders.Add(_currentOrder);
                dbContext.Placements.AddRange(placements);
            }
            else
            {
                dbContext.Entry(_currentOrder).State = EntityState.Modified;
            }

            try
            {
                await dbContext.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                ErrorMessage = ex.Message;
                return false;
            }

            return true;
        }
        public IEnumerable<InsuranceDto> GetUsedInsurances()
        {
            return _insurancesDto;
        }
        public string ErrorMessage { get; private set; }
        public async Task<IEnumerable<OrderDto>> GetAllOrders(int clientId, ValuteGetterService valuteGetter)
        {
            await dbContext.Orders.LoadAsync();

            var list = await (dbContext.Orders.
                Where(x => x.ClientId == clientId && (x.OrderStatus == OrderStatus.Active || x.OrderStatus == OrderStatus.Completed)).
                OrderBy(x => x.CreationDate)).
                ToListAsync();

            foreach(var order in list)
            {
                await dbContext.Entry(order).Collection(x => x.Insurances).LoadAsync();
                await dbContext.Entry(order).Reference(x => x.Tour).LoadAsync();
                await dbContext.Entry(order).Reference(x => x.Client).LoadAsync();
            }

            return list.Select(y =>
            {
                var inst = mapper.MapTo<Order, OrderDto>(y);
                inst.ClientDto = mapper.MapTo<Client, ClientDto>(y.Client);
                inst.FullCostEUR = valuteGetter.GetEuroValue(y.FullCost);
                inst.FullCostUSD = valuteGetter.GetUSDValue(y.FullCost);
                inst.InsuranceDtos = y.Insurances.Select(x => 
                {
                    var dto = mapper.MapTo<Insurance, InsuranceDto>(x);
                    dto.CostUSD = valuteGetter.GetUSDValue(x.Cost);
                    dto.CostEUR = valuteGetter.GetEuroValue(x.Cost);

                    return dto;
                }).ToList();

                var tour = dbContext.Tours.AsNoTracking().FirstOrDefault(x => x.Id == y.TourId);
                inst.TourDto = mapper.MapTo<Tour, TourDto>(tour);
                
                return inst;
            });
        }
        public async Task<IEnumerable<PlacementDto>> GetPlacements(int orderId)
        {
            await dbContext.Placements.Include(x => x.Cabin).LoadAsync();

            var list = await dbContext.Placements.Where(x => x.OrderId == orderId).ToListAsync();

            foreach(var pl in list)
            {
                await dbContext.Entry(pl).Reference(x => x.Cabin).LoadAsync();
            }

            return list.Select(x => 
            {
                var dto = mapper.MapTo<Placement, PlacementDto>(x);
                dto.CabinDto = mapper.MapTo<Cabin, CabinDto>(x.Cabin);
                return dto;
            });
        }
        public async Task<OrderDto> CancelOrder(int orderId)
        {
            await dbContext.Orders.LoadAsync();
            var order = await dbContext.Orders.FindAsync(orderId);

            order.OrderStatus = OrderStatus.Canceled;
            dbContext.Entry(order).State = EntityState.Modified;
            await dbContext.SaveChangesAsync();
            return mapper.MapTo<Order, OrderDto>(order);
        }
    }
}
