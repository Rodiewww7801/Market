using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Market.Data.Entities.OrderAggregate;
using Market.Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Market.EF.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly MarketContext _context;
        public OrderRepository(MarketContext context)
        {
            _context = context;
        }
        public void Create(Order order)
        {
            _context.Orders.Add(order);
            _context.SaveChanges();
        }

        public IEnumerable<Order> GetAllOrders()
        {
            return _context.Orders.Include(x => x.OrderDetails).Include(x => x.OrderItems).ToList();
        }

        public Order GetOrder(int orderId)
        {
            return _context.Orders.Include(x => x.OrderDetails).Include(x => x.OrderItems).FirstOrDefault(x => x.Id == orderId);
        }

        public void Remove(Order order)
        {
            _context.Orders.Attach(order);
            _context.Orders.Remove(order);
            _context.SaveChanges();
        }

        public void Update(Order order)
        {
            _context.Orders.Attach(order);
            _context.Entry(order).State = EntityState.Modified;
            _context.SaveChanges();
        }
    }
}
