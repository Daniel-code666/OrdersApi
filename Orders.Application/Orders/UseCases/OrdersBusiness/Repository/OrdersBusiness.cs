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
        private readonly IRabbitMqPublisher _rabbitMqPublisher;
        private readonly IMapper _mapper;

        public OrdersBusiness(IMapper mapper, IOrdersRepository ordersRepository, IRabbitMqPublisher rabbitMqPublisher)
        {
            _ordersRepository = ordersRepository;
            _mapper = mapper;
            _rabbitMqPublisher = rabbitMqPublisher;
        }

        async Task<DbActions> IOrdersBusiness.CreateOrder(OrdersCreate order)
        {
            var result = await _ordersRepository.CreateOrder(_mapper.Map<OrdersEntity>(order));

            if (result == 0)
                return DbActions.NotCreated;

            var order_created = new OrdersCreated
            {
                OrderId = result,
                OrderNumber = order.OrderNumber,
                CustomerName = order.CustomerName,
                CustomerEmail = order.CustomerEmail,
                CustomerPhone = order.CustomerPhone,
                TotalAmount = order.TotalAmount,
                TotalFee = order.TotalFee,
                OrderState = order.OrderState,
                Active = order.Active,
                CreatedAt = DateTime.UtcNow
            };

            await _rabbitMqPublisher.PublishOrderCreatedAsync(order_created);

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
