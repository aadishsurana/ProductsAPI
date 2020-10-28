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
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace RefactorThis.Tests
{
    public class ProductOptionServiceTests
    {
        private readonly Mock<IProductOptionRepository> _mockOptionRepo;
        private readonly Mock<IProductRepository> _mockProductRepo;
        private readonly IMapper _mapper;
        private readonly IFixture _fixture;
        private readonly IProductOptionService _sut;

        public ProductOptionServiceTests()
        {
            _mockOptionRepo = new Mock<IProductOptionRepository>();
            _mockProductRepo = new Mock<IProductRepository>();
            _mapper = new MapperConfiguration(c => c.AddProfile<MappingProfile>()).CreateMapper();
            _fixture = new Fixture().Customize(new AutoMoqCustomization());
            _sut = new ProductOptionService(_mapper, _mockOptionRepo.Object, _mockProductRepo.Object);
        }

        [Fact]
        public async Task Test_GetAllProductOptions()
        {
            //Arrange
            var listOfPO = _fixture.Create<List<ProductOptionEntity>>();
            var productId = Guid.NewGuid();
            _mockOptionRepo.Setup(x => x.GetProductOptions(productId)).ReturnsAsync(listOfPO);

            //Act
            var response = await _sut.GetAllProductOptions(productId);

            //Assert
            Assert.Equal(listOfPO.Count, response.Count);
        }

        [Fact]
        public async Task Test_GetProductOptionById_ReturnsOption()
        {
            //Arrange
            var optionId = Guid.NewGuid();
            var productOption = _fixture.Create<ProductOptionEntity>();
            productOption.Id = optionId.ToString();
            var productId = Guid.NewGuid();
            _mockOptionRepo.Setup(x => x.GetProductOptionById(productId, optionId)).ReturnsAsync(productOption);

            //Act
            var response = await _sut.GetProductOptionById(productId, optionId);

            //Assert
            Assert.Equal(productOption.Id, response.Id);
        }

        [Fact]
        public async Task Test_GetProductOptionById_ReturnsNull_WhenOptionNotFound()
        {
            //Arrange
            var optionId = Guid.NewGuid();
            var productOption = _fixture.Create<ProductOptionEntity>();
            productOption.Id = optionId.ToString();
            var productId = Guid.NewGuid();
            _mockOptionRepo.Setup(x => x.GetProductOptionById(productId, optionId)).ReturnsAsync((ProductOptionEntity)null);

            //Act
            var response = await _sut.GetProductOptionById(productId, optionId);

            //Assert
            Assert.Null(response);
        }

        [Fact]
        public async Task Test_CreateOption_SuccessfullyCreatesProductOption()
        {
            //Arrange
            var searchedProduct = new ProductEntity
            {
                Id = Guid.NewGuid().ToString(),
                Name = "pName",
                Description = "pDesc",
                DeliveryPrice = 11.0m,
                Price = 1000.0m
            };
            var optionToCreate = new ProductOptionEntity
            {
                ProductId = searchedProduct.Id,
                Name = "poName",
                Description = "poDesc"
            };

            _mockProductRepo.Setup(x => x.GetProductById(Guid.Parse(searchedProduct.Id))).ReturnsAsync(searchedProduct);
            _mockOptionRepo.Setup(x => x.CreateOption(It.IsAny<ProductOptionEntity>())).ReturnsAsync(optionToCreate);

            //Act
            var mappedOption = _mapper.Map<ProductOption>(optionToCreate);
            var response = await _sut.CreateOption(mappedOption);

            //Assert
            Assert.Equal(optionToCreate.Name, response.Name);
        }

        [Fact]
        public async Task Test_CreateOption_ThrowsError_WhenProvidedProduct_IsNotValid()
        {
            //Arrange
            var searchedProduct = new ProductEntity
            {
                Id = Guid.NewGuid().ToString(),
                Name = "pName",
                Description = "pDesc",
                DeliveryPrice = 11.0m,
                Price = 1000.0m
            };
            var optionToCreate = new ProductOptionEntity
            {
                ProductId = Guid.NewGuid().ToString(),
                Name = "poName",
                Description = "poDesc"
            };

            _mockProductRepo.Setup(x => x.GetProductById(Guid.Parse(searchedProduct.Id))).ReturnsAsync((ProductEntity)null);

            //Assert
            var mappedOption = _mapper.Map<ProductOption>(optionToCreate);
            var errorMsg = await Assert.ThrowsAsync<Exception>(() => _sut.CreateOption(mappedOption));
            Assert.Equal("Product doesn't exist in system", errorMsg.Message);
        }

        [Fact]
        public async Task Test_UpdateOption_SuccessfullyUpdatesProduct()
        {
            //Arange
            var searchedProduct = new ProductEntity
            {
                Id = Guid.NewGuid().ToString(),
                Name = "pName",
                Description = "pDesc",
                DeliveryPrice = 11.0m,
                Price = 1000.0m
            };
            var origOption = new ProductOptionEntity
            {
                Id = Guid.NewGuid().ToString(),
                ProductId = Guid.NewGuid().ToString(),
                Name = "poName",
                Description = "poDesc"
            };

            var optionToUpdate = new ProductOptionEntity
            {
                Id = origOption.Id,
                ProductId = searchedProduct.Id,
                Name = "poName",
                Description = "poDesc"
            };
            _mockOptionRepo.Setup(x => x.GetProductOptionById(Guid.Parse(searchedProduct.Id), Guid.Parse(origOption.Id))).ReturnsAsync(origOption);
            _mockOptionRepo.Setup(x => x.UpdateOption(It.IsAny<ProductOptionEntity>())).ReturnsAsync(1);

            //Act
            var mappedOption = _mapper.Map<ProductOption>(optionToUpdate);
            var response = await _sut.UpdateOption(mappedOption);

            //Assert
            Assert.Equal(1, response);
        }

        [Fact]
        public async Task Test_UpdateOption_ThrowsError_WhenInvalidOptionIsPassed()
        {
            //Arange
            var searchedProduct = new ProductEntity
            {
                Id = Guid.NewGuid().ToString(),
                Name = "pName",
                Description = "pDesc",
                DeliveryPrice = 11.0m,
                Price = 1000.0m
            };
            var origOption = new ProductOptionEntity
            {
                Id = Guid.NewGuid().ToString(),
                ProductId = Guid.NewGuid().ToString(),
                Name = "poName",
                Description = "poDesc"
            };

            var optionToUpdate = new ProductOptionEntity
            {
                Id = origOption.Id,
                ProductId = searchedProduct.Id,
                Name = "poName",
                Description = "poDesc"
            };
            _mockOptionRepo.Setup(x => x.GetProductOptionById(Guid.Parse(searchedProduct.Id), Guid.Parse(origOption.Id))).ReturnsAsync((ProductOptionEntity)null);

            //Assert
            var mappedOption = _mapper.Map<ProductOption>(optionToUpdate);
            var errorMsg = await Assert.ThrowsAsync<Exception>(() => _sut.UpdateOption(mappedOption));
            Assert.Equal("Provided product option doesn't exist in system", errorMsg.Message);
        }

        [Fact]
        public async Task Test_DeleteProduct_SuccessfullyDeletesOption()
        {
            //Arrange
            var searchedProduct = new ProductEntity
            {
                Id = Guid.NewGuid().ToString(),
                Name = "pName",
                Description = "pDesc",
                DeliveryPrice = 11.0m,
                Price = 1000.0m
            };
            var optionToDelete = new ProductOptionEntity
            {
                Id = Guid.NewGuid().ToString(),
                ProductId = searchedProduct.Id,
                Name = "poName",
                Description = "poDesc"
            };

            _mockOptionRepo.Setup(x => x.GetProductOptionById(Guid.Parse(searchedProduct.Id), Guid.Parse(optionToDelete.Id))).ReturnsAsync(optionToDelete);
            _mockOptionRepo.Setup(x => x.DeleteOption(It.IsAny<ProductOptionEntity>())).Returns(1);

            //Act
            var response = await _sut.DeleteOption(Guid.Parse(searchedProduct.Id), Guid.Parse(optionToDelete.Id));

            //Assert
            Assert.Equal(1, response);
        }

        [Fact]
        public async Task Test_DeleteProduct_ThrowsError_WhenInvalidOptionIsPassed()
        {
            //Arrange
            var searchedProduct = new ProductEntity
            {
                Id = Guid.NewGuid().ToString(),
                Name = "pName",
                Description = "pDesc",
                DeliveryPrice = 11.0m,
                Price = 1000.0m
            };
            var optionToDelete = new ProductOptionEntity
            {
                Id = Guid.NewGuid().ToString(),
                ProductId = Guid.NewGuid().ToString(),
                Name = "poName",
                Description = "poDesc"
            };

            _mockOptionRepo.Setup(x => x.GetProductOptionById(Guid.Parse(searchedProduct.Id), Guid.Parse(optionToDelete.Id))).ReturnsAsync((ProductOptionEntity)null);

            //Assert
            var errorMsg = await Assert.ThrowsAsync<Exception>(() => _sut.DeleteOption(Guid.Parse(searchedProduct.Id), Guid.Parse(optionToDelete.Id)));
            Assert.Equal("Provided product option doesn't exist in system", errorMsg.Message);
        }
    }
}
