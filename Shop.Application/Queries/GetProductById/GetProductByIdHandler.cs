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

            var entity = await _repository.GetProductByIdAsync(request.Id, cancellationToken);
            
            return _mapper.Map<ProductDto?>(entity);
        }
    }
}
