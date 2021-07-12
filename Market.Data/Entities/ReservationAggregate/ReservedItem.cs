using System;
using System.Collections.Generic;
using System.Text;

namespace Market.Data.Entities.ReservationAggregate
{
    public class ReservedItem
    {
        public int Id { get; set; }
        public int ReservationId { get; set; }
        public int ProductId { get; set; }
        public int ReservedQuantity { get; set; }
        public Reservation Reservation { get; set; }
    }
}
