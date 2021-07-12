using Market.Data.Entities.OrderAggregate;
using Market.Data.Entities.ProductAggregate;
using Market.Data.Entities.ReservationAggregate;
using Market.Data.Repositories;
using Market.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Market.Domain.Implementations
{
    public class ReservationService : IReservationService
    {
        private Mutex mutex = new Mutex(false, "MarketMutext");
        private IReservationRepository _reservationRepository;
        private IProductRepository _productRepository;
       
        public ReservationService(IReservationRepository reservationRepository, IProductRepository productRepository)
        {
            _reservationRepository = reservationRepository;
            _productRepository = productRepository;
        }
        public bool IsActive(int reservationId)
        {
            var reservation = _reservationRepository.GetReservation(reservationId);
            if(reservation == null)
                return false;
            return reservation.TimeStart < DateTime.Now && reservation.TimeEnd > DateTime.Now;
        }

        public void RemoveReservation(int reservationId)
        {
            var reservation = _reservationRepository.GetReservation(reservationId);
            foreach(var item in reservation.ReservedItems)
                _productRepository.AddQuantity(item.ProductId, item.ReservedQuantity);
            _reservationRepository.Remove(reservation);
        }
        public List<Product> OutOfStock(Order order, out bool result )
        {
            //condition
            mutex.WaitOne();
            var productOutOfStock = new List<Product>();
            foreach(var item in order.OrderItems)
            {
                var product = _productRepository.GetProduct(item.ProductId);
                if ( item.Quantity > product.QuantityInStock ) 
                    productOutOfStock.Add(product);
            }
            if (productOutOfStock.Count != 0)
                result = true;
            else
                result = false;
            mutex.ReleaseMutex();
            return productOutOfStock;
        }
        public void ReservationCheker()
        {
            var reservList = _reservationRepository.GetAllReservations();
            foreach (var reserv in reservList)
                if (!IsActive(reserv.Id) && reserv.OrderId == null)
                {
                    foreach (var item in reserv.ReservedItems)
                        _productRepository.AddQuantity(item.ProductId, item.ReservedQuantity);
                    _reservationRepository.Remove(reserv);
                }
        }

        public Reservation Reserve(Order order)
        {
            var reservation = new Reservation()
            {
                TimeStart = DateTime.Now,
                TimeEnd = DateTime.Now.AddMinutes(5),
                ReservedItems = new List<ReservedItem>()
            };

            foreach(var item in order.OrderItems)
            {
                var reservedItem = new ReservedItem()
                {
                    ProductId = item.ProductId,
                    ReservedQuantity = item.Quantity,
                    Reservation = reservation,
                    ReservationId = reservation.Id 
                };
                reservation.ReservedItems.Add(reservedItem);
                _productRepository.RemoveQuantity(reservedItem.ProductId, reservedItem.ReservedQuantity);
            }
            _reservationRepository.Create(reservation);
            order.ReservationId = reservation.Id;
            return reservation;
        }
        
    }
}
