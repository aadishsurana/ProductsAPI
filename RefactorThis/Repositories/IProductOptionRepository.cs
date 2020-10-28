using RefactorThis.Repositories.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RefactorThis.Repositories
{
    public interface IProductOptionRepository
    {
        Task<List<ProductOptionEntity>> GetProductOptions(Guid productId); 
        Task<ProductOptionEntity> GetProductOptionById(Guid productId, Guid optionId);
        Task<ProductOptionEntity> CreateOption(ProductOptionEntity option);
        Task<int> UpdateOption(ProductOptionEntity option);
        int DeleteOption(ProductOptionEntity product);
    }
}
