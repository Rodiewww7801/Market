using Market.Data.Entities.OrderAggregate;
using Market.Data.Entities.ProductAggregate;
using Market.Data.Entities.ReservationAggregate;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace Market.Mongo
{
    public class MongoContext
    {
        private readonly IMongoDatabase database;
        public MongoContext(string connectionString)
        {
            MongoClient client = new MongoClient(connectionString);
            var connectionDB = new MongoUrlBuilder(connectionString);
            database = client.GetDatabase(connectionDB.DatabaseName);
        }

        public IMongoCollection<Product> Products
        {
            get
            {
                return database.GetCollection<Product>("Products");
            }
        }

        public IMongoCollection<Order> Orders
        {
            get
            {
                return database.GetCollection<Order>("Orders");
            }
        }
        public IMongoCollection<Reservation> Reservations
        {
            get
            {
                return database.GetCollection<Reservation>("Reservations");
            }
        }

       
    }
}
