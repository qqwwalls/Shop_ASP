using MediatR;
using Shop.Application.DTOs;
using Shop.Application.Interfaces.Services;
using System.Threading;
using System.Threading.Tasks;

namespace Shop.Application.Queries.GetProductById
{
    public class GetProductByIdHandler : IRequestHandler<GetProductByIdQuery, ProductDto?>
    {
        private readonly IProductService _productService;

        public GetProductByIdHandler(IProductService productService)
        {
            _productService = productService;
        }

        public Task<ProductDto?> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            // Використовуємо існуючий сервіс для отримання продукту
            // Якщо у майбутньому ви захочете повністю відмовитись від IProductService,
            // тут можна напряму викликати IProductRepository та AutoMapper.
            var product = _productService.GetProductById(request.Id);
            return Task.FromResult(product);
        }
    }
}
