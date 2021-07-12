using System;
using System.Collections.Generic;
using System.Text;
using Market.Data.Entities.ProductAggregate;
using Market.Data.Repositories;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Linq;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace Market.Mongo.Repositories
{
    public class ProductRepositoryMongo: IProductRepository
    {

        private readonly MongoContext _mongoContext;
        public ProductRepositoryMongo(MongoContext mongoContext)
        {
            _mongoContext = mongoContext;
        }

        public void AddQuantity(int productId, int quantity)
        {
            var product = GetProduct(productId);
            product.QuantityInStock += quantity;
            _mongoContext.Products.ReplaceOne(new BsonDocument("_id", productId), product);
        }

        public void RemoveQuantity(int productId, int quantity)
        {
            var product = GetProduct(productId);
            product.QuantityInStock -= quantity;
            _mongoContext.Products.ReplaceOne(new BsonDocument("_id", productId), product);
        }
        public  IEnumerable<string> GetAllCategories()
        {

            var category = _mongoContext.Products.Distinct(p => p.Category, Builders<Product>.Filter.Empty).ToList();
            category.Sort();
            return category;
        }

        public IEnumerable<Product> GetAllProducts()
        {
            return _mongoContext.Products.Find(new BsonDocument()).ToList();
        }

        public Product GetProduct(int id)
        {
            return _mongoContext.Products.Find(new BsonDocument("_id", id)).FirstOrDefaultAsync().Result;   
        }

        public IEnumerable<Product> GetProductsSelectedCategory(string category)
        {
            return  _mongoContext.Products.Find(new BsonDocument("Category", category)).ToList();
        }

       

    }
}
