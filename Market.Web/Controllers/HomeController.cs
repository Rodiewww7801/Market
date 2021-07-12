using Market.Data.Entities.ProductAggregate;
using Market.Data.Repositories;
using Market.Domain.Interfaces;
using Market.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Market.Web.Controllers
{
    public class HomeController : Controller
    {
        private IProductRepository _productRepository;

        public HomeController(IProductRepository productRepository)
        {

            _productRepository = productRepository;
        }

        public ViewResult ProductList()
        {
            return View(_productRepository.GetAllProducts());
        } 

        public ViewResult CategoryList(string category)
        {

            var model = new ProductListViewModel()
            {
                Products = _productRepository.GetProductsSelectedCategory(category),
                SelectedCategory = category 
            };
            return View(model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
