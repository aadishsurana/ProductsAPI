using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RefactorThis.Models;
using RefactorThis.Services;

namespace RefactorThis.Controllers
{
    [Route("api/products")]
    [ApiController]
    public class ProductOptionsController : ControllerBase
    {
        private readonly IProductOptionService _optionService;
        private readonly IMapper _mapper;

        public ProductOptionsController(IProductOptionService optionService, IMapper mapper)
        {
            _optionService = optionService;
            _mapper = mapper;
        }

        [HttpGet("{productId}/options")]
        public async Task<List<ProductOption>> GetOptions(Guid productId)
        {
            var productOptions = await _optionService.GetAllProductOptions(productId);
            return productOptions;
        }

        [HttpGet("{productId}/options/{optionId}")]
        public async Task<ProductOption> GetOption(Guid productId, Guid optionId)
        {
            var productOption = await _optionService.GetProductOptionById(productId, optionId);
            return productOption;
        }

        [HttpPost("{productId}/options")]
        public async Task<IActionResult> CreateOption(Guid productId, [FromBody] CreateUpdateOptionRequest request)
        {
            var optionRequest = _mapper.Map<ProductOption>(request);
            optionRequest.ProductId = productId.ToString();
            try
            {
                var optionResponse = await _optionService.CreateOption(optionRequest);
                return new OkObjectResult(optionResponse);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(new { error = ex.Message });
            }
        }

        [HttpPut("{productId}/options/{optionId}")]
        public async Task<IActionResult> UpdateOption(Guid productId, Guid optionId, [FromBody] CreateUpdateOptionRequest request)
        {
            var updateRequest = _mapper.Map<ProductOption>(request);
            updateRequest.ProductId = productId.ToString();
            updateRequest.Id = optionId.ToString();
            try
            {
                var optionResponse = await _optionService.UpdateOption(updateRequest);
                return Ok();
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(new { error = ex.Message });
            }
        }

        [HttpDelete("{productId}/options/{optionId}")]
        public async Task<IActionResult> DeleteOption(Guid productId, Guid optionId)
        {
            try
            {
                var optionDeleted = await _optionService.DeleteOption(productId, optionId);
                return Ok();
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(new { error = ex.Message });
            }
        }
    }
}
