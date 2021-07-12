using Market.Data.Entities.OrderAggregate;
using Market.Data.Repositories;
using Market.Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Market.Web.Provider
{
    public class CartProvider : ICartProvider
    {
        private IHttpContextAccessor _httpContext;
        private ISession _session => _httpContext.HttpContext.Session;
        public CartProvider(IHttpContextAccessor httpContext)
        {
            _httpContext = httpContext;

        }
        public Order GetSessionCart()
        {
            var order = _session.GetObjectFromJson<Order>("Cart");
            if(order == null)
            {
                order = new Order();
                order.OrderItems = new List<OrderItem>();
                _session.SetObjectAsJson("Cart", order);
            }
            return order;
        }

        public void UpdateSessionCart(Order order)
        {
            _session.SetObjectAsJson("Cart", order);
        }
    }
}
