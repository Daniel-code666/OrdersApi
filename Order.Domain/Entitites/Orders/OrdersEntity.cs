using Order.Domain.Entitites.Base;
using Order.Domain.Entitites.Enums.OrdersEnum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Order.Domain.Entitites.Orders
{
    [Table("Orders")]
    public class OrdersEntity : AuditTable
    {
        [Key]
        public int Id { get; set; }
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
