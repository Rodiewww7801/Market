using Market.Data.Entities.OrderAggregate;
using Market.Data.Repositories;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace Market.Mongo.Repositories
{
    public class OrderRepositoryMongo: IOrderRepository
    {
        private readonly MongoContext _mongoContext;
        public OrderRepositoryMongo(MongoContext mongoContext)
        {
            _mongoContext = mongoContext;
        }

        public void Create(Order order)
        {
            _mongoContext.Orders.InsertOne(order);
        }

        public IEnumerable<Order> GetAllOrders()
        {
            return _mongoContext.Orders.Find(Builders<Order>.Filter.Empty).ToList();
        }

        public Order GetOrder(int orderId)
        {
            return _mongoContext.Orders.Find(new BsonDocument("_id", orderId)).FirstOrDefaultAsync().Result;
        }

        public void Remove(Order order)
        {
            _mongoContext.Orders.DeleteOne(new BsonDocument("_id", order.Id));
        }

        public void Update(Order order)
        {
            _mongoContext.Orders.ReplaceOne(new BsonDocument("_id", order.Id), order);
        }
    }
}
