using Market.Data.Entities.ProductAggregate;
using Market.Data.Repositories;
using Market.Domain.Interfaces;
using Market.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Market.Web.Controllers
{
    public class CartController : Controller
    {
        private Mutex mutex = new Mutex(false, "MarketMutext");
        private ICartProvider _cartProvider;
        private IProductRepository _productRepository;
        private ICartService _cartService;

        public CartController(ICartProvider cartProvider, ICartService cartService, IProductRepository productRepository)
        {
            _cartProvider = cartProvider;
            _cartService = cartService;
            _productRepository = productRepository;
        }

        public RedirectToActionResult AddToCart(int productId, string returnUrl)
        {
            //condition
            mutex.WaitOne();
            var product = _productRepository.GetProduct(productId);
            if (product != null && product.QuantityInStock > 0)
                _cartService.AddItem(productId, 1);
            else
                ModelState.AddModelError("OutOfStock", $"{product.Name} out of stock. Available in stock:{product.QuantityInStock}");
            mutex.ReleaseMutex();
            return RedirectToAction("Index", new { returnUrl });
        }

        public PartialViewResult ShowCart(string returnUrl)
        {
            var products = new List<Product>();
            var orderItems = _cartService.GetOrderItems();
            foreach(var item in orderItems)
                products.Add(_productRepository.GetProduct(item.ProductId));
            return PartialView(new CartViewModel()
            {
                Products =products,
                OrderItems = _cartService.GetOrderItems(),
                TotatPrice = _cartService.TotalPrice(),
                IsValid = ModelState.IsValid,
                ReturnUrl = returnUrl
            });
        }

        public RedirectToActionResult RemoveFromCart(int productId, string returnUrl)
        {
            var product = _productRepository.GetProduct(productId);
            if(product != null)
                _cartService.RemoveOrderItem(productId);
            return RedirectToAction("Index", new { returnUrl });
        }

        public ViewResult Index(string returnUrl)
        {
            mutex.WaitOne();
            var productList = new List<Product>();
            var cart = _cartProvider.GetSessionCart();
            if (cart.OrderItems.Count == 0)
                ModelState.AddModelError("EmptyCart", "Your cart is empty");

            foreach (var item in cart.OrderItems)
            {
                //condition
                var product = _productRepository.GetProduct(item.ProductId);
                productList.Add(product);
                if (item.Quantity > product.QuantityInStock)
                    ModelState.AddModelError("OutOfStock", $"{product.Name} out of stock. Available in stock:{product.QuantityInStock}");

            }
            mutex.ReleaseMutex();
            return View(new CartViewModel
            {
                Products = productList,
                OrderItems = _cartService.GetOrderItems(),
                TotatPrice = _cartService.TotalPrice(),
                IsValid = ModelState.IsValid,
                ReturnUrl = returnUrl
            });

        }
    }
}
