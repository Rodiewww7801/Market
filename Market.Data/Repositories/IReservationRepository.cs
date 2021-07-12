using Market.Data.Entities.ReservationAggregate;
using System;
using System.Collections.Generic;
using System.Text;

namespace Market.Data.Repositories
{
    public interface IReservationRepository
    {
        Reservation GetReservation(int id);
        IEnumerable<Reservation> GetAllReservations();
        void Create(Reservation reservation);
        void Update(Reservation reservation);
        void Remove(Reservation reservation);


    }
}
