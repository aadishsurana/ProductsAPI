using RefactorThis.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RefactorThis.Services
{
    public interface IProductService
    {
        Task<List<Product>> GetAllProducts();
        Task<Product> GetProductById(Guid id);
        Task<Product> CreateProduct(Product product);
        Task<int> UpdateProduct(Product product);
        Task<int> DeleteProduct(Product product);
    }
}
