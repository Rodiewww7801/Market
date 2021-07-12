using Market.Data.Entities.OrderAggregate;
using Market.Data.Entities.ProductAggregate;
using Market.Data.Entities.ReservationAggregate;
using System;
using System.Collections.Generic;
using System.Text;

namespace Market.Domain.Interfaces
{
    public interface IReservationService
    {
        Reservation Reserve(Order order);
        void RemoveReservation(int reservationId);
        List<Product> OutOfStock(Order order, out bool result );
        void ReservationCheker();
        bool IsActive(int reservationId);
    }
}
