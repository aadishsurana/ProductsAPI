using Microsoft.AspNetCore.Mvc;
using NPoco;
using RefactorThis.DatabaseFactory;
using RefactorThis.Models;
using RefactorThis.Repositories.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RefactorThis.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly IDbFactory _dbFactory;

        public ProductRepository(IDbFactory dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public async Task<List<ProductEntity>> GetProducts()
        {
            using (var db = _dbFactory.GetConnection())
            {
                return await db.FetchAsync<ProductEntity>("select * from Products");
            }
        }

        public async Task<ProductEntity> GetProductById(Guid id)
        {
            using (var db = _dbFactory.GetConnection())
            {
                return await db.SingleOrDefaultAsync<ProductEntity>("select * from Products where id = @0 collate nocase", id);
            }
        }

        public async Task<ProductEntity> CreateProduct(ProductEntity product)
        {
            using (var db = _dbFactory.GetConnection())
            {
                product.Id = Guid.NewGuid().ToString();
                await db.InsertAsync(product);
                return product;
            }
        }

        public async Task<int> UpdateProduct(ProductEntity product)
        {
            using (var db = _dbFactory.GetConnection())
            {
                return await db.UpdateAsync(product);
            }
        }

        public int DeleteProduct(ProductEntity product)
        {
            using (var db = _dbFactory.GetConnection())
            {
                return db.Delete<ProductEntity>(product);
            }

        }
    }
}
