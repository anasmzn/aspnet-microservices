using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Catalog.Api.Entities;

namespace Catalog.Api.Repository
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllProducts();
        Task<Product> GetProduct(string id);
        Task<IEnumerable<Product>> GetProductsByCategory(string category);
        Task<IEnumerable<Product>> GetProductsByName(string name);
        Task CreateProduct(Product product);
        Task<bool> UpdateProduct(Product product);
        Task<bool> DeleteProduct(string id);
    }
}
