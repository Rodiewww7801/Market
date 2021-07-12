using System;
using System.Collections.Generic;
using System.Text;

namespace Market.Data.Entities.ReservationAggregate
{
    public class Reservation
    {
        public int Id { get; set; }
        public DateTime TimeStart { get; set; }
        public DateTime TimeEnd { get; set; }
        public List<ReservedItem> ReservedItems { get; set; }
        public int? OrderId { get; set; } // Product is first reserved and then added to the Order. If the OrderDetail has not been filled and paid up, the OrderId is NULL

    }
}
