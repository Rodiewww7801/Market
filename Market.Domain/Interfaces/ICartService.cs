using Market.Data.Entities.OrderAggregate;
using System;
using System.Collections.Generic;
using System.Text;

namespace Market.Domain.Interfaces
{
    public interface ICartService
    {
        void AddItem(int productId, int quantity);
        void RemoveOrderItem(int productId);
        decimal TotalPrice();
        void ClearCart();
        List<OrderItem> GetOrderItems();
    }
}
