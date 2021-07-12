using Market.Data.Entities.OrderAggregate;
using System;
using System.Collections.Generic;
using System.Text;

namespace Market.Domain.Interfaces
{
    public interface ICartProvider
    {
        Order GetSessionCart();
        void UpdateSessionCart(Order order);
    }
}
