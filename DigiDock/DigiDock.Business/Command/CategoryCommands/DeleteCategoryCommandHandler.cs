using DigiDock.Base.Responses;
using DigiDock.Business.Cqrs;
using DigiDock.Data.UnitOfWork;
using MediatR;

namespace DigiDock.Business.Command.CategoryCommands
{
    public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand, ApiResponse>
    {
        private readonly IUnitOfWork unitOfWork;
        public DeleteCategoryCommandHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<ApiResponse> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = await unitOfWork.CategoryRepository.FirstOrDefault(c => c.Id == request.CategoryId);
            if (category is null)
            {
                return ApiResponse.ErrorResponse("Category not found");
            }

            await unitOfWork.CategoryRepository.Delete(category.Id);
            await unitOfWork.CompleteAsync();
            return ApiResponse.SuccessResponse("Category deleted successfully");
        }
    }
}
