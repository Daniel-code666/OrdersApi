using AutoMapper;
using Order.Domain.Entitites.Base;
using Order.Domain.Entitites.Orders;
using Orders.Application.Interfaces;
using Orders.Application.Orders.Dtos;
using Orders.Application.Orders.UseCases.OrdersBusiness.Interfaces;

namespace Orders.Application.Orders.UseCases.OrdersBusiness.Repository
{
    public class OrdersBusiness : IOrdersBusiness
    {
        private readonly IOrdersRepository _ordersRepository;
        private readonly IMapper _mapper;

        public OrdersBusiness(IMapper mapper, IOrdersRepository ordersRepository)
        {
            _ordersRepository = ordersRepository;
            _mapper = mapper;
        }

        async Task<DbActions> IOrdersBusiness.CreateOrder(OrdersCreate order)
        {
            var result = await _ordersRepository.CreateOrder(_mapper.Map<OrdersEntity>(order));

            if (!result)
                return DbActions.NotCreated;

            return DbActions.Created;
        }

        async Task<OrdersRead?> IOrdersBusiness.GetById(int id)
            => _mapper.Map<OrdersRead?>(await _ordersRepository.GetOrderById(id));

        async Task<IEnumerable<OrdersRead>> IOrdersBusiness.GetAll(OrdersFilters filters)
        {
            var result = await _ordersRepository.GetAllOrders(filters);

            return _mapper.Map<IEnumerable<OrdersRead>>(result);
        }
    }
}
