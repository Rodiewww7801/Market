using System;
using System.Collections.Generic;
using System.Text;

namespace Market.Data.Entities.OrderAggregate
{
    public enum OrderStatusEnum { Active = 1, PaidUp = 2}
    public class Order
    {
        public int Id { get; set; }
        public OrderStatusEnum Status { get; set; }
        public DateTime OrderDate { get; set; }
        public int ReservationId { get; set; }
        public OrderDetails OrderDetails { get; set; }
        public List<OrderItem> OrderItems { get; set; }

    }
}
