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
    public class ProductOptionRepository : IProductOptionRepository
    {
        private readonly IDbFactory _dbFactory;

        public ProductOptionRepository(IDbFactory dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public async Task<List<ProductOptionEntity>> GetProductOptions(Guid productId)
        {
            using (var db = _dbFactory.GetConnection())
            {
                return await db.FetchAsync<ProductOptionEntity>("select * from ProductOptions where ProductId = @0 collate nocase", productId);
            }
        }

        public async Task<ProductOptionEntity> GetProductOptionById(Guid productId, Guid optionId)
        {
            using (var db = _dbFactory.GetConnection())
            {
                return await db.SingleOrDefaultAsync<ProductOptionEntity>("select * from ProductOptions where productId = @0 and Id = @1 collate nocase", productId.ToString(), optionId.ToString());
            }
        }

        public async Task<ProductOptionEntity> CreateOption(ProductOptionEntity option)
        {
            using (var db = _dbFactory.GetConnection())
            {
                option.Id = Guid.NewGuid().ToString();
                await db.InsertAsync(option);
                return option;
            }
        }

        public async Task<int> UpdateOption(ProductOptionEntity option)
        {
            using (var db = _dbFactory.GetConnection())
            {
                return await db.UpdateAsync(option);
            }
        }

        public int DeleteOption(ProductOptionEntity option)
        {
            using (var db = _dbFactory.GetConnection())
            {
                return db.Delete<ProductOptionEntity>(option);
            }
        }
    }
}
