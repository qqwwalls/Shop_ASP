using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using Shop.Api.Controllers;
using Shop.Application.Interfaces.Services;
using Shop.Application.DTOs;
using Shop.Api.Interfaces;
using FluentValidation;

namespace Shop.Test
{
    public class CategoryControllerTest
    {
        private readonly Mock<ICategoryService> _mockCategoryService;
        private readonly Mock<IImageService> _mockImageService;
        private readonly Mock<StackExchange.Redis.IConnectionMultiplexer> _mockRedis;
        private readonly Mock<IValidator<CategoryCreateDTO>> _mockValidator;
        private readonly CategoryController _controller;

        public CategoryControllerTest()
        {
            _mockCategoryService = new Mock<ICategoryService>();
            _mockImageService = new Mock<IImageService>();
            _mockRedis = new Mock<StackExchange.Redis.IConnectionMultiplexer>();
            _mockValidator = new Mock<IValidator<CategoryCreateDTO>>();

            _controller = new CategoryController(
                _mockCategoryService.Object,
                _mockImageService.Object,
                _mockRedis.Object,
                _mockValidator.Object
            );
        }

        [Fact]
        public async Task GetCategoryById_ReturnsOkResult_WhenCategoryExists()
        {
            // Arrange
            int categoryId = 1;
            var categoryDto = new CategoryReadDTO { Id = categoryId, Name = "Test Category" };
            
            _mockCategoryService
                .Setup(service => service.GetCategoryByIdAsync(categoryId, default))
                .ReturnsAsync(categoryDto);

            // Act
            var result = await _controller.GetCategoryById(categoryId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<CategoryReadDTO>(okResult.Value);
            Assert.Equal(categoryId, returnValue.Id);
        }

        [Fact]
        public async Task GetCategoryById_ReturnsNotFoundResult_WhenCategoryDoesNotExist()
        {
            // Arrange
            int categoryId = 99;
            
            _mockCategoryService
                .Setup(service => service.GetCategoryByIdAsync(categoryId, default))
                .ReturnsAsync((CategoryReadDTO?)null);

            // Act
            var result = await _controller.GetCategoryById(categoryId);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }
    }
}
