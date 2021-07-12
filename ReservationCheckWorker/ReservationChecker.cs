using Market.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ReservationCheckWorker
{
    public class ReservationChecker
    {
        private Mutex mutex = new Mutex(false, "MarketMutext");
        private IReservationRepository _reservationRepository;
        private IProductRepository _productRepository;
        public ReservationChecker(IReservationRepository reservationRepository, IProductRepository productRepository)
        {
            _reservationRepository = reservationRepository;
            _productRepository = productRepository;
        }
        public async Task DoWork(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                mutex.WaitOne();
                var reservList = _reservationRepository.GetAllReservations();
                foreach (var reserv in reservList)
                    if (DateTime.Now > reserv.TimeEnd && reserv.Deleted == false)
                    {
                        foreach (var item in reserv.ReservedItems)
                            _productRepository.AddQuantity(item.ProductId, item.ReservedQuantity);
                        reserv.Deleted = true;
                        _reservationRepository.Update(reserv);
                    }
                mutex.ReleaseMutex();
                await Task.Delay(5000, cancellationToken); // 5s
            }
        }
    }
}
