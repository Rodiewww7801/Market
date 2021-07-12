using Market.Data.Entities.ProductAggregate;
using System;
using System.Collections.Generic;
using System.Text;

namespace Market.Data.Repositories
{
    public interface IProductRepository
    {
        Product GetProduct(int id);
        IEnumerable<Product> GetAllProducts();
        IEnumerable<Product> GetProductsSelectedCategory(string category);
        IEnumerable<string> GetAllCategories();
        void RemoveQuantity(int productId, int quantity);
        void AddQuantity(int productId, int quantity);

    }
}
