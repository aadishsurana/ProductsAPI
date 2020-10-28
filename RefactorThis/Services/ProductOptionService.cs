using AutoMapper;
using RefactorThis.Models;
using RefactorThis.Repositories;
using RefactorThis.Repositories.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RefactorThis.Services
{
    public class ProductOptionService : IProductOptionService
    {
        private readonly IMapper _mapper;
        private readonly IProductOptionRepository _poRepository;
        private readonly IProductRepository _productRepository;

        public ProductOptionService(IMapper mapper, IProductOptionRepository poRepository, IProductRepository productRepository)
        {
            _mapper = mapper;
            _poRepository = poRepository;
            _productRepository = productRepository;
        }

        public async Task<List<ProductOption>> GetAllProductOptions(Guid productId)
        {
            var entityProductOptions = await _poRepository.GetProductOptions(productId);
            var allProductOptions = _mapper.Map<List<ProductOption>>(entityProductOptions);
            return allProductOptions;
        }

        public async Task<ProductOption> GetProductOptionById(Guid productId, Guid optionId)
        {
            var entityProductOption = await _poRepository.GetProductOptionById(productId, optionId);
            var selectedOption = _mapper.Map<ProductOption>(entityProductOption);
            return selectedOption;
        }

        public async Task<ProductOption> CreateOption(ProductOption option)
        {
            var existingProduct = await _productRepository.GetProductById(Guid.Parse(option.ProductId));
            if (existingProduct == null)
                throw new Exception("Product doesn't exist in system");

            var newOption = _mapper.Map<ProductOptionEntity>(option);
            var entityOption = await _poRepository.CreateOption(newOption);
            var response = _mapper.Map<ProductOption>(entityOption);
            return response;
        }

        public async Task<int> UpdateOption(ProductOption option)
        {
            var existingOption = await _poRepository.GetProductOptionById(Guid.Parse(option.ProductId), Guid.Parse(option.Id));
            if (existingOption == null)
                throw new Exception("Provided product option doesn't exist in system");

            var optionToUpdate = _mapper.Map<ProductOptionEntity>(option);
            int optionUpdated = await _poRepository.UpdateOption(optionToUpdate);
            return optionUpdated;
        }

        public async Task<int> DeleteOption(Guid productId, Guid optionId)
        {
            var existingOption = await _poRepository.GetProductOptionById(productId, optionId);
            if (existingOption == null)
                throw new Exception("Provided product option doesn't exist in system");

            int optionDeleted = _poRepository.DeleteOption(existingOption);
            return optionDeleted;
        }
    }
}

