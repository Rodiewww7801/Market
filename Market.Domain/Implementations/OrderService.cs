using Market.Data.Entities.OrderAggregate;
using Market.Data.Repositories;
using Market.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Market.Domain.Implementations
{
    public class OrderService : IOrderService
    {
        private ICartProvider _cartProvider;
        private IOrderRepository _orderRepository;
        public OrderService(ICartProvider cartProvider, IOrderRepository orderRepository)
        {
            _cartProvider = cartProvider;
            _orderRepository = orderRepository;
        }

        public Order CreateOrder(OrderDetails orderDetails)
        {
            var order = _cartProvider.GetSessionCart();
            order.OrderDetails = orderDetails;
            order.OrderDate = DateTime.Now;
            order.Status = OrderStatusEnum.PaidUp;
            _orderRepository.Create(order);
            return order;
        }
    }
}
