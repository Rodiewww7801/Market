using Market.Data.Entities.OrderAggregate;
using Market.Data.Entities.ProductAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Market.Web.Models
{
    public class CartViewModel
    {
        public List<Product> Products { get; set; }
        public List<OrderItem> OrderItems { get; set; }
        public decimal TotatPrice { get; set; }
        public bool IsValid { get; set; }
        public string ReturnUrl { get; set; }
    }
}
