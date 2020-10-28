using AutoFixture;
using AutoFixture.AutoMoq;
using AutoMapper;
using Moq;
using RefactorThis.Mapping;
using RefactorThis.Models;
using RefactorThis.Repositories;
using RefactorThis.Repositories.Entities;
using RefactorThis.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace RefactorThis.Tests
{
    public class ProductServiceTests
    {
        private readonly Mock<IProductRepository> _mockProductRepo;
        private readonly Mock<IProductOptionService> _mockOptionService;
        private readonly IMapper _mapper;
        private readonly IFixture _fixture;
        private readonly IProductService _sut;

        public ProductServiceTests()
        {
            _mockProductRepo = new Mock<IProductRepository>();
            _mockOptionService = new Mock<IProductOptionService>();
            _mapper = new MapperConfiguration(c => c.AddProfile<MappingProfile>()).CreateMapper();
            _fixture = new Fixture().Customize(new AutoMoqCustomization());
            _sut = new ProductService(_mapper, _mockProductRepo.Object, _mockOptionService.Object);
        }

        [Fact]
        public async Task Test_GetAllProducts()
        {
            //Arrange
            var listOfProducts = _fixture.Create<List<ProductEntity>>();
            _mockProductRepo.Setup(x => x.GetProducts()).ReturnsAsync(listOfProducts);

            //Act
            var response = await _sut.GetAllProducts();

            //Assert
            Assert.Equal(listOfProducts.Count, response.Count);
        }

        [Fact]
        public async Task Test_GetProductById_ReturnsProduct()
        {
            //Arrange
            var productGuid = Guid.NewGuid();
            var product = _fixture.Create<ProductEntity>();
            product.Id = productGuid.ToString();
            _mockProductRepo.Setup(x => x.GetProductById(productGuid)).ReturnsAsync(product);

            //Act
            var response = await _sut.GetProductById(productGuid);

            //Assert
            Assert.Equal(product.Id, response.Id);
        }

        [Fact]
        public async Task Test_GetProductById_ReturnsNull_WhenProductNotFound()
        {
            //Arrange
            var productGuid = Guid.NewGuid();
            var product = _fixture.Create<ProductEntity>();
            product.Id = productGuid.ToString();
            _mockProductRepo.Setup(x => x.GetProductById(productGuid)).ReturnsAsync((ProductEntity)null);

            //Act
            var response = await _sut.GetProductById(productGuid);

            //Assert
            Assert.Null(response);
        }

        [Fact]
        public async Task Test_CreateProduct_SuccessfullyCreatesProduct()
        {
            //Arrange
            var productToCreate = _fixture.Create<ProductEntity>();
            _mockProductRepo.Setup(x => x.CreateProduct(It.IsAny<ProductEntity>())).ReturnsAsync(productToCreate);
            var createdProduct = _mapper.Map<Product>(productToCreate);

            //Act
            var response = await _sut.CreateProduct(createdProduct);

            //Assert
            Assert.Equal(createdProduct.Id, response.Id);
            Assert.Equal(createdProduct.Description, response.Description);
        }

        [Fact]
        public async Task Test_CreateProduct_ReturnsNull_WhenFailedToCreateProduct()
        {
            //Arrange
            var productToCreate = _fixture.Create<ProductEntity>();
            _mockProductRepo.Setup(x => x.CreateProduct(It.IsAny<ProductEntity>())).ReturnsAsync((ProductEntity)null);
            var createdProduct = _mapper.Map<Product>(productToCreate);

            //Act
            var response = await _sut.CreateProduct(createdProduct);

            //Assert
            Assert.Null(response);
        }

        [Fact]
        public async Task Test_UpdateProduct()
        {
            //Arrange
            var origProduct = _fixture.Create<Product>();
            var updateRequest = new ProductEntity
            {
                Id = origProduct.Id,
                Description = "Desc update",
                Name = "xName",
                DeliveryPrice = origProduct.DeliveryPrice,
                Price = origProduct.Price
            };
            _mockProductRepo.Setup(x => x.UpdateProduct(It.IsAny<ProductEntity>())).ReturnsAsync(1);

            //Act
            var mappedProduct = _mapper.Map<Product>(updateRequest);
            var response = await _sut.UpdateProduct(mappedProduct);

            //Assert
            Assert.Equal(1, response);
        }

        [Fact]
        public async Task Test_DeleteProduct()
        {
            //Arrange
            var productToDelete = _fixture.Create<Product>();
            productToDelete.Id = Guid.NewGuid().ToString();

            var productOptions = new List<ProductOption>
            {
                new ProductOption
                {
                    Id = Guid.NewGuid().ToString(),
                    ProductId = productToDelete.Id,
                    Description = "po1",
                    Name = "poName"
                }
            };

            _mockOptionService.Setup(x => x.GetAllProductOptions(It.IsAny<Guid>())).ReturnsAsync(productOptions);
            _mockOptionService.Setup(x => x.DeleteOption(It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync(It.IsAny<int>());
            _mockProductRepo.Setup(x => x.DeleteProduct(It.IsAny<ProductEntity>())).Returns(1);

            //Act
            var response = await _sut.DeleteProduct(productToDelete);

            //Assert
            Assert.Equal(1, response);
        }
    }
}
