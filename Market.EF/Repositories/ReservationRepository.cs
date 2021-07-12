using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Market.Data.Entities.ReservationAggregate;
using Market.Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Market.EF.Repository
{
    public class ReservationRepository: IReservationRepository
    {
        private readonly MarketContext _context;

        public ReservationRepository(MarketContext context)
        {
            _context = context;
        }

        public void Create(Reservation reservation)
        {
            _context.Reservations.Add(reservation);
            _context.SaveChanges();
        }

        public IEnumerable<Reservation> GetAllReservations()
        {
            return _context.Reservations.Include(p => p.ReservedItems).ToList();
        }

        public Reservation GetReservation(int id)
        {
            return _context.Reservations.Include(p => p.ReservedItems).FirstOrDefault(x => x.Id == id);

        }

        public void Remove(Reservation reservation)
        {
            _context.Reservations.Attach(reservation);
            _context.Reservations.Remove(reservation);
            _context.SaveChanges();
        }

        public void Update(Reservation reservation)
        {
            _context.Reservations.Attach(reservation);
            _context.Entry(reservation).State = EntityState.Modified;
            _context.SaveChanges();
        }
    }
}
