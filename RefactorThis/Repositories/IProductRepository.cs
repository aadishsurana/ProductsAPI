using RefactorThis.Repositories.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RefactorThis.Repositories
{
    public interface IProductRepository
    {
        Task<List<ProductEntity>> GetProducts(); 
        Task<ProductEntity> GetProductById(Guid id);
        Task<ProductEntity> CreateProduct(ProductEntity product);
        Task<int> UpdateProduct(ProductEntity product);
        int DeleteProduct(ProductEntity product);
    }
}
