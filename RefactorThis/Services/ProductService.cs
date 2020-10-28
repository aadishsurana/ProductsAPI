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
    public class ProductService : IProductService
    {
        private readonly IMapper _mapper;
        private readonly IProductRepository _productRepository;
        private readonly IProductOptionService _optionService;

        public ProductService(IMapper mapper, IProductRepository productRepository, IProductOptionService optionService)
        {
            _mapper = mapper;
            _productRepository = productRepository;
            _optionService = optionService;
        }

        public async Task<List<Product>> GetAllProducts()
        {
            var entityProducts = await _productRepository.GetProducts();
            var allProducts = _mapper.Map<List<Product>>(entityProducts);
            return allProducts;
        }

        public async Task<Product> GetProductById(Guid id)
        {
            var entityProduct = await _productRepository.GetProductById(id);
            var selectedProduct = _mapper.Map<Product>(entityProduct);
            return selectedProduct;
        }

        public async Task<Product> CreateProduct(Product product)
        {
            var newProduct = _mapper.Map<ProductEntity>(product);
            var entityProduct = await _productRepository.CreateProduct(newProduct);
            var response = _mapper.Map<Product>(entityProduct);
            return response;
        }

        public async Task<int> UpdateProduct(Product product)
        {
            var productToUpdate = _mapper.Map<ProductEntity>(product);
            int productUpdated = await _productRepository.UpdateProduct(productToUpdate);
            return productUpdated;
        }

        public async Task<int> DeleteProduct(Product product)
        {
            var productToDelete = _mapper.Map<ProductEntity>(product);
            var allOptions = await _optionService.GetAllProductOptions(Guid.Parse(productToDelete.Id));
            if (allOptions.Count > 0)
            {
                foreach (var option in allOptions)
                {
                    await _optionService.DeleteOption(Guid.Parse(productToDelete.Id), Guid.Parse(option.Id));
                }
            }
            int productDeleted = _productRepository.DeleteProduct(productToDelete);
            return productDeleted;
        }
    }
}

