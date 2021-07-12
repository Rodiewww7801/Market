using System;
using System.Collections.Generic;
using System.Text;

namespace Market.Data.Entities.OrderAggregate
{
    public class OrderItem
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public Order Order { get; set; }

    }
}
