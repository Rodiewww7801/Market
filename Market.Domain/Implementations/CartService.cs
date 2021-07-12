using Market.Data.Entities.OrderAggregate;
using Market.Data.Repositories;
using Market.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Market.Domain.Implementations
{
    public class CartService : ICartService
    {
        private ICartProvider _cartProvider;
        private IProductRepository _productRepository;
        public CartService(ICartProvider cartProvider, IProductRepository productRepository)
        {
            _cartProvider = cartProvider;
            _productRepository = productRepository;
        }
        public void ClearCart()
        {
            _cartProvider.GetSessionCart().OrderItems.Clear();
        }

        public void AddItem(int productId, int quantity)
        {
            var cart = _cartProvider.GetSessionCart();
            var orderItem = cart.OrderItems.FirstOrDefault(x => x.ProductId == productId);
            if (orderItem == null)
            {
                cart.OrderItems.Add(new OrderItem()
                {
                    ProductId = productId,
                    Quantity = quantity
                });
            }
            else
                orderItem.Quantity += quantity;
            _cartProvider.UpdateSessionCart(cart);
        }

        public List<OrderItem> GetOrderItems()
        {
            return _cartProvider.GetSessionCart().OrderItems.ToList();
        }

        public void RemoveOrderItem(int productId)
        {
            var cart = _cartProvider.GetSessionCart();
            cart.OrderItems.RemoveAll(x => x.ProductId == productId);
            _cartProvider.UpdateSessionCart(cart);
        }

        public decimal TotalPrice()
        {
            decimal totalPrice = 0;
            var orderItems = _cartProvider.GetSessionCart().OrderItems;
            foreach(var item in orderItems)
            {   
                totalPrice += _productRepository.GetProduct(item.ProductId).Price * item.Quantity;
            }
            return totalPrice;
        }
    }
}
