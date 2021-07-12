using Market.Data.Entities.OrderAggregate;
using Market.Data.Repositories;
using Market.Domain.Interfaces;
using Market.Web.Provider;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Market.Domain.Logger;

namespace Market.Web.Controllers
{
    public class OrderController : Controller
    {
        private  ICartProvider _cartProvider;
        private IReservationService _reservationService;
        private IReservationRepository _reservationRepository;
        private IOrderService _orderService;
        private ILogger _logger;

        //private ILogger logger = Logger.CreateLogger<OrderController>();
        public OrderController(ICartProvider cartProvider,
            IReservationService reservationService,
            IReservationRepository reservationRepsoitory,
            IOrderService orderService,
            ILogger<OrderController> logger)
        {
            _cartProvider = cartProvider;
            _reservationService = reservationService;
            _reservationRepository = reservationRepsoitory;
            _orderService = orderService;
            _logger = logger;
        }
        
        public RedirectToActionResult Reservation()
        {

            var cart = _cartProvider.GetSessionCart();
            if (cart.OrderItems.Count == 0)
                return RedirectToAction("Index", "Cart");
            bool result;
            var outOfStockProducts = _reservationService.OutOfStock(cart, out result);
            if (!result)
            {
                _reservationService.Reserve(cart);
                _cartProvider.UpdateSessionCart(cart);
            }
            else
            {
                foreach (var product in outOfStockProducts)
                    ModelState.AddModelError("OutOfStock", $"{product.Name} out of stock. Available in stock:{product.QuantityInStock}");
                return RedirectToAction("Index", "Cart");
            }
            return RedirectToAction("Checkout");
        }
        public ActionResult Checkout()
        {
            return View(new OrderDetails());
        }

        [HttpPost]
        public ViewResult Checkout(OrderDetails orderDetails)
        {
            var cart = _cartProvider.GetSessionCart();
            var reservation = _reservationRepository.GetReservation(cart.ReservationId);
            if (ModelState.IsValid)
            {
                if (_reservationService.IsActive(cart.ReservationId))
                {
                    var order = _orderService.CreateOrder(orderDetails);
                    reservation.OrderId = order.Id;
                    reservation.Deleted = true;
                    _reservationRepository.Update(reservation);
                    cart = new Order() { OrderItems = new List<OrderItem>() };
                    _cartProvider.UpdateSessionCart(cart);
                    _logger.LogInformation($"Order: Id:{order.Id}, Date:{order.OrderDate}, Status:{order.Status}");
                    return View("Complete");
                }
                else {
                    return View("TimeOut");
                }
            }
            return View(orderDetails);
        }

        public ActionResult Index()
        {
            return View();
        }
    }
}
