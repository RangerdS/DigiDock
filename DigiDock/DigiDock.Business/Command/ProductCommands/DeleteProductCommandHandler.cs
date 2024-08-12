using DigiDock.Base.Responses;
using DigiDock.Business.Cqrs;
using DigiDock.Data.UnitOfWork;
using MediatR;

namespace DigiDock.Business.Command.ProductCommands
{
    public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, ApiResponse>
    {
        private readonly IUnitOfWork unitOfWork;
        public DeleteProductCommandHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<ApiResponse> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var product = await unitOfWork.ProductRepository.GetByIdAsync(request.ProductId);
            if (product == null)
            {
                return ApiResponse.ErrorResponse("Product not found");
            }

            unitOfWork.ProductRepository.Delete(product);
            await unitOfWork.CompleteAsync();

            return ApiResponse.ErrorResponse("Product deleted successfully");
        }
    }
}
