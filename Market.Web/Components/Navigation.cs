using Market.Data.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Market.Web.Components
{

    public class Navigation: ViewComponent
    {
        private IProductRepository _productRepository;
        public Navigation(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public IViewComponentResult Invoke()
        {

            return View(_productRepository.GetAllCategories());
        }
    }
}
