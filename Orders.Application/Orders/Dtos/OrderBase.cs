using Order.Domain.Entitites.Enums.OrdersEnum;

namespace Orders.Application.Orders.Dtos
{
    public class OrderBase
    {
        public int? Id { get; set; }
        public string OrderNumber { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerEmail { get; set; } = string.Empty;
        public string CustomerPhone { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
        public decimal TotalFee { get; set; }
        public OrderState OrderState { get; set; }
        public bool Active { get; set; } = true;
    }
}
