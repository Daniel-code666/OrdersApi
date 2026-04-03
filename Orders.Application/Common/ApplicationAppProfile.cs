using AutoMapper;
using Order.Domain.Entitites.Orders;
using Orders.Application.Orders.Dtos;

namespace Orders.Application.Common
{
    public class ApplicationAppProfile : Profile
    {
        public ApplicationAppProfile()
        {
            MapOrders();
        }

        private void MapOrders()
        {
            CreateMap<OrdersEntity, OrdersRead>();
            CreateMap<OrdersCreate, OrdersEntity>();
            CreateMap<OrdersEntity, OrdersCreated>()
                .ForMember(dest => dest.OrderId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreationDate));
        }
    }
}
