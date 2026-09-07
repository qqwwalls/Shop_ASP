using MediatR;

namespace Shop.Application.Commands.DeleteProduct
{
    public record DeleteProductCommand(int Id) : IRequest<bool>;
}
