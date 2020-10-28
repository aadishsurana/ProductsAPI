using RefactorThis.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RefactorThis.Services
{
    public interface IProductOptionService
    {
        Task<List<ProductOption>> GetAllProductOptions(Guid productId);
        Task<ProductOption> GetProductOptionById(Guid productId, Guid optionId);
        Task<ProductOption> CreateOption(ProductOption option);
        Task<int> UpdateOption(ProductOption option);
        Task<int> DeleteOption(Guid productId, Guid optionId);
    }
}
