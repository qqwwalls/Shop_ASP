using AutoMapper;
using MediatR;
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

        public GetProductByIdHandler(IProductRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ProductDto?> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            // Отримуємо сутність з БД через репозиторій (як на скріншоті)
            var entity = _repository.GetProductById(request.Id);
            
            // Мапимо в DTO і повертаємо
            return await Task.FromResult(_mapper.Map<ProductDto?>(entity));
        }
    }
}
