using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Catalog.Api.Data;
using Catalog.Api.Entities;
using MongoDB.Driver;

namespace Catalog.Api.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly ICatalogContext _catalogContext;

        public ProductRepository(ICatalogContext catalogContext)
        {
            this._catalogContext = catalogContext;
        }

        public async Task CreateProduct(Product product)
        {
            await _catalogContext.Products.InsertOneAsync(product);
        }

        public async Task<bool> DeleteProduct(string id)
        {
            var deleteResult = await _catalogContext.Products.DeleteOneAsync(x => x.Id == id);
            return deleteResult.IsAcknowledged && deleteResult.DeletedCount > 0;
        }

        public async Task<IEnumerable<Product>> GetAllProducts()
        {
            var filter = Builders<Product>.Filter.Empty;
            var filteredProducts = await _catalogContext.
                Products.
                //Find(x => true)
                FindAsync(filter);
            return await filteredProducts.ToListAsync();
        }

        public async Task<Product> GetProduct(string id)
        {
            var filter = Builders<Product>.Filter.Eq(p => p.Id, id);
            var filteredProducts = await _catalogContext.
                Products.
                FindAsync(filter);
            return await filteredProducts.FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Product>> GetProductsByCategory(string category)
        {
            var filter = Builders<Product>.Filter.Eq(p => p.Category, category);
            var filteredProducts = await _catalogContext.
                Products.
                FindAsync(filter);
            return await filteredProducts.ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProductsByName(string name)
        {
            var filter = Builders<Product>.Filter.Eq(p => p.Name, name);
            var filteredProducts = await _catalogContext.
                Products.
                FindAsync(filter);
            return await filteredProducts.ToListAsync();
        }

        public async Task<bool> UpdateProduct(Product product)
        {
            var result = await _catalogContext.Products
                                    .ReplaceOneAsync(p => p.Id == product.Id, product);
            return result.IsAcknowledged && result.ModifiedCount > 0;
        }
    }
}
