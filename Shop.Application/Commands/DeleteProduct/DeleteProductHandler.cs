using MediatR;
using Microsoft.Extensions.Logging;
using Shop.Application.Interfaces.Repository;
using System.Threading;
using System.Threading.Tasks;

namespace Shop.Application.Commands.DeleteProduct
{
    public class DeleteProductHandler : IRequestHandler<DeleteProductCommand, bool>
    {
        private readonly IProductRepository _repository;
        private readonly ILogger<DeleteProductHandler> _logger;

        public DeleteProductHandler(IProductRepository repository, ILogger<DeleteProductHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<bool> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("💥 [CQRS] DeleteProductHandler was triggered to delete Product ID: {Id}", request.Id);
            
            var entity = await _repository.GetProductByIdAsync(request.Id, cancellationToken);
            if (entity == null)
            {
                return false;
            }

            await _repository.DeleteProductAsync(entity, cancellationToken);
            return true;
        }
    }
}
