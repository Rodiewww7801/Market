using Market.Data.Entities.OrderAggregate;
using Market.Data.Entities.ProductAggregate;
using Market.Data.Repositories;
using Market.Domain.Interfaces;
using Market.Web.Controllers;
using Market.Web.Models;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Market.Tests
{
    public class CartControllerTests
    {
        [Fact]
        public void RemoveFromCartReturnsRedirectAndRemoveFromCart()
        {
            //Arrange
            var product = new Product { Id = 1, QuantityInStock = 1 };
            var mockCartProvider = Mock.Of<ICartProvider>();
            var mockCartService = new Mock<ICartService>();
            var mockProductRepository = Mock.Of<IProductRepository>(x => x.GetProduct(It.IsAny<int>()) == product);

            var cartController = new CartController(mockCartProvider, mockCartService.Object, mockProductRepository);

            //Act
            var actionName = cartController.RemoveFromCart(product.Id, "returnUrl").ActionName;

            //Assert
            Assert.Equal("Index", actionName);
            mockCartService.Verify(x => x.RemoveOrderItem(product.Id));
        }

        [Fact]
        public void AddToCartReturnsARedirectAndAddToCart()
        {
            //Arrange
            var product = new Product { Id = 1, QuantityInStock = 1 };
            var mockCartProvider = Mock.Of<ICartProvider>();
            var mockCartService = new  Mock<ICartService>();
            var mockProductRepository = Mock.Of<IProductRepository>(x => x.GetProduct(It.IsAny<int>()) ==product);

            var cartController = new CartController(mockCartProvider, mockCartService.Object, mockProductRepository);

            //Act
            var actionName = cartController.AddToCart(product.Id, "returnUrl").ActionName;

            //Assert
            Assert.Equal("Index", actionName);
            mockCartService.Verify(x => x.AddItem(product.Id, 1));
        }

        [Fact]
        public void IndexReturnsAViewResult()
        {
            //Arrange
            var mockCartProvider = new Mock<ICartProvider>();
            mockCartProvider.Setup(x => x.GetSessionCart()).Returns(new Order() 
            { OrderItems = new List<OrderItem>() 
                { new OrderItem() { ProductId = 1}
            } });
            var mockCartService = new Mock<ICartService>();
            var mockProductRepository = Mock.Of<IProductRepository>(x => x.GetProduct(It.IsAny<int>()) == new Product());
            var cartController = new CartController(mockCartProvider.Object, mockCartService.Object, mockProductRepository);

            //Act
            var model = cartController.Index("returnUrl").Model as CartViewModel;
            //Assert

            Assert.NotNull(model);
            Assert.NotEmpty(model.Products);
            Assert.True(model.IsValid);

        }
    }
}
