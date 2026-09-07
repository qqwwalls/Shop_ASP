using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Shop.Application.DTOs;
using Shop.Application.Interfaces.Repository;
using System.Threading;
using System.Threading.Tasks;

namespace Shop.Application.Queries.GetProductById
{
    public class GetProductByIdHandler : IRequestHandler<GetProductByIdQuery, ProductDto?>
    {
        private readonly IProductRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetProductByIdHandler> _logger;

        public GetProductByIdHandler(IProductRepository repository, IMapper mapper, ILogger<GetProductByIdHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ProductDto?> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("🚀 [CQRS] GetProductByIdHandler was triggered for Product ID: {Id}", request.Id);

            // Отримуємо сутність з БД через репозиторій (як на скріншоті)
            var entity = _repository.GetProductById(request.Id);
            
            // Мапимо в DTO і повертаємо
            return await Task.FromResult(_mapper.Map<ProductDto?>(entity));
        }
    }
}
