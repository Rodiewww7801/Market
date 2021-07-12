using Market.Data.Entities.OrderAggregate;
using Market.Data.Entities.ProductAggregate;
using Market.Data.Entities.ReservationAggregate;
using Market.Data.Repositories;
using Market.Domain.Interfaces;
using Market.Web.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Market.Tests
{
    public class OrderControllerTests
    {
        private Order _sessionCart;

        [Fact]
        public void ReservationReturnsRedirectToIndex()
        {
            //Arrange
            var mockCartProvider = new Mock<ICartProvider>();
            var mockLogger = new Mock<ILogger<OrderController>>();
            mockCartProvider.Setup(x => x.GetSessionCart()).Returns(_sessionCart = new Order()
            {
                OrderItems = new List<OrderItem>()
                { new OrderItem() { ProductId = 1}
            }
            });
            var mockReservationService = new Mock<IReservationService>();
            bool result = true;
            mockReservationService.Setup(x => x.OutOfStock(_sessionCart, out result)).Returns(new List<Product>() { new Product { Name = "name", QuantityInStock = 1} });
            var mockReservationRepository = new Mock<IReservationRepository>();
            var mockOrderService = new Mock<IOrderService>();
            var cartController = new OrderController(
                mockCartProvider.Object,
                mockReservationService.Object,
                mockReservationRepository.Object,
                mockOrderService.Object,
                mockLogger.Object
                );

            //Act
            var actionName = cartController.Reservation().ActionName;

            //Assert
            Assert.Equal("Index", actionName);
        }

        [Fact]
        public void ReservationReturnsRedirectToCheckoutAndReserv()
        {
            //Arrange
            var mockCartProvider = new Mock<ICartProvider>();
            var mockLogger = new  Mock<ILogger<OrderController>>();
            mockCartProvider.Setup(x => x.GetSessionCart()).Returns(_sessionCart =  new Order()
            {
                OrderItems = new List<OrderItem>()
                { new OrderItem() { ProductId = 1}
            }
            });
            var mockReservationService = new Mock<IReservationService>();
            var mockReservationRepository = new Mock<IReservationRepository>();
            var mockOrderService = new Mock<IOrderService>();
            var cartController = new OrderController(
                mockCartProvider.Object,
                mockReservationService.Object,
                mockReservationRepository.Object,
                mockOrderService.Object,
                mockLogger.Object
                );

            //Act
            var actionName = cartController.Reservation().ActionName;

            //Assert
            Assert.Equal("Checkout", actionName);
            mockReservationService.Verify(x => x.Reserve(_sessionCart));

        }

        [Fact]
        public void CheckoutCreateOrder()
        {
            //Arrange
            
            var mockCartProvider = new Mock<ICartProvider>();
            mockCartProvider.Setup(x => x.GetSessionCart()).Returns(_sessionCart = new Order()
            {
                ReservationId = 1,
                OrderItems = new List<OrderItem>()
                { new OrderItem() { ProductId = 1}
            }
            });

            var order = new Order() { Id = 1, ReservationId = _sessionCart.ReservationId };
            var orderDetail = new OrderDetails();

            var mockLogger = new Mock<ILogger<OrderController>>();
            var mockReservationService = new Mock<IReservationService>();
            mockReservationService.Setup(x => x.IsActive(_sessionCart.ReservationId)).Returns(true);
         
            var mockReservationRepository = new Mock<IReservationRepository>();
            mockReservationRepository.Setup(x => x.GetReservation(_sessionCart.ReservationId)).Returns(new Reservation());
            var mockOrderService = new Mock<IOrderService>();
            mockOrderService.Setup(x => x.CreateOrder(orderDetail)).Returns(order);
            var cartController = new OrderController(
                mockCartProvider.Object,
                mockReservationService.Object,
                mockReservationRepository.Object,
                mockOrderService.Object,
                mockLogger.Object
                );

            //Act
            var result = cartController.Checkout(orderDetail);

            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("Complete", viewResult.ViewName);
            
        }
    }
}
