using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Market.Data.Entities.ProductAggregate;
using Market.Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Market.EF.Repository
{
    public class ProductRepository : IProductRepository
    {
        
        private readonly MarketContext _context;
        public ProductRepository(MarketContext context)
        {
            _context = context;
        }

        public void AddQuantity(int productId, int quantity)
        {
            
            var product = GetProduct(productId);
            _context.Attach(product);
            product.QuantityInStock += quantity;
            _context.Entry(product).Property(p => p.QuantityInStock).IsModified = true;
            _context.SaveChanges();
        }

        public void RemoveQuantity(int productId, int quantity)
        {
            
            var product = GetProduct(productId);
            _context.Attach(product);
            product.QuantityInStock -= quantity;
            _context.Entry(product).Property(p => p.QuantityInStock).IsModified = true;
            _context.SaveChanges();
        }

        public IEnumerable<string> GetAllCategories()
        {
            
            return _context.Products.Select(s => s.Category).Distinct().OrderBy(o=>o).ToList();
        }

        public IEnumerable<Product> GetAllProducts()
        {
            
            return _context.Products.ToList();
        }

        public Product GetProduct(int id)
        {
            
            return _context.Products.Where(x => x.Id == id).FirstOrDefault();
        }

        public IEnumerable<Product> GetProductsSelectedCategory(string category)
        {
            
            return _context.Products.Where(c => c.Category == category).ToList();
        }
    }
}
