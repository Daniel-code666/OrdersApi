using Orders.Worker.Orders.Enums;

namespace Orders.Worker.Orders
{
    public class OrdersCreated
    {
        public int OrderId { get; set; }
        public string OrderNumber { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerEmail { get; set; } = string.Empty;
        public string CustomerPhone { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
        public decimal TotalFee { get; set; }
        public OrderState OrderState { get; set; }
        public bool Active { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
