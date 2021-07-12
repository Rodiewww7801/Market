using Market.Data.Entities.ReservationAggregate;
using Market.Data.Repositories;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace Market.Mongo.Repositories
{
    public class ReservationRepositoryMongo : IReservationRepository
    {
        private readonly MongoContext _mongoContext;
        public ReservationRepositoryMongo(MongoContext mongoContext)
        {
            _mongoContext = mongoContext;
        }
        public void Create(Reservation reservation)
        {
            foreach(var item in reservation.ReservedItems)
            {
                item.Reservation = null;
            }
            _mongoContext.Reservations.InsertOne(reservation);
        }

        public IEnumerable<Reservation> GetAllReservations()
        {
            return _mongoContext.Reservations.Find(Builders<Reservation>.Filter.Empty).ToList();
        }

        public Reservation GetReservation(int id)
        {
            return _mongoContext.Reservations.Find(new BsonDocument("_id", id)).FirstOrDefault();
        }

        public void Remove(Reservation reservation)
        {
            _mongoContext.Reservations.DeleteOne(new BsonDocument("_id", reservation.Id));
        }

        public void Update(Reservation reservation)
        {
            _mongoContext.Reservations.ReplaceOne(new BsonDocument("_id", reservation.Id), reservation);
        }
    }
}
