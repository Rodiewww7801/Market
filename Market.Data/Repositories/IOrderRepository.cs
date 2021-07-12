using Market.Data.Entities.OrderAggregate;
using System;
using System.Collections.Generic;
using System.Text;

namespace Market.Data.Repositories
{
    public interface IOrderRepository
    {
        IEnumerable<Order> GetAllOrders();
        Order GetOrder(int orderId);
        void Create(Order order);
        void Update(Order order);
        void Remove(Order order);

    }
}
