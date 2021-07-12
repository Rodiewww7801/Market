using Market.Data.Entities.ProductAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Market.Web.Models
{
    public class ProductListViewModel
    {
        public IEnumerable<Product> Products { get; set; }
        public string SelectedCategory { get; set; }
    }
}
