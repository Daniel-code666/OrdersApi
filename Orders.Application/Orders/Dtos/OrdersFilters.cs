using Order.Domain.Entitites.Enums.OrdersEnum;

namespace Orders.Application.Orders.Dtos
{
    public class OrdersFilters
    {
        public string? OrderNumber { get; set; }
        public string? CustomerName { get; set; }
        public string? CustomerEmail { get; set; }
        public string? CustomerPhone { get; set; }
        public DateTime? CreationDate { get; set; }
        public DateTime? ModificationDate { get; set; }
        public OrderState? OrderState { get; set; }
        public bool? Active { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}
