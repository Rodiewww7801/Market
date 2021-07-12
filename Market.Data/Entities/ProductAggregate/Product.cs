using System;
using System.Collections.Generic;
using System.Text;

namespace Market.Data.Entities.ProductAggregate
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Info { get; set; }
        public string Category { get; set; }
        public int QuantityInStock { get; set; }
        public decimal Price { get; set; }
    }
}
