using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RefactorThis.Models;
using RefactorThis.Services;

namespace RefactorThis.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IMapper _mapper;

        public ProductsController(IProductService productService, IMapper mapper)
        {
            _productService = productService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<List<Product>> GetProducts()
        {
            var products = await _productService.GetAllProducts();
            return products;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(Guid id)
        {
            var requestedProduct = await _productService.GetProductById(id);
            if(requestedProduct == null)
            {
                return NotFound();
            }
            return Ok(requestedProduct);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] CreateUpdateProductRequest request)
        {
            var productRequest = _mapper.Map<Product>(request);
            var productResponse = await _productService.CreateProduct(productRequest);
            if (productResponse == null)
                return BadRequest();

            return new OkObjectResult(productResponse);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(Guid id, [FromBody] CreateUpdateProductRequest request)
        {
            var validProduct = await _productService.GetProductById(id);
            if (validProduct == null)
                return BadRequest();

            var updateRequest = _mapper.Map<Product>(request);
            updateRequest.Id = id.ToString();
            int recordsUpdated = await _productService.UpdateProduct(updateRequest);
            if (recordsUpdated == 0)
                return NotFound();

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(Guid id)
        {
            var validProduct = await _productService.GetProductById(id);
            if (validProduct == null)
                return BadRequest();

            validProduct.Id = id.ToString(); 
            int productDeleted = await _productService.DeleteProduct(validProduct);
            return Ok();
        }
    }
}