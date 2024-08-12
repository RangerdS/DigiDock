using AutoMapper;
using DigiDock.Base.Responses;
using DigiDock.Business.Cqrs;
using DigiDock.Data.Domain;
using DigiDock.Data.UnitOfWork;
using MediatR;

namespace DigiDock.Business.Command.CategoryCommands
{
    public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, ApiResponse>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public UpdateCategoryCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<ApiResponse> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = await unitOfWork.CategoryRepository.GetByIdAsync(request.Request.Id);
            if (category == null)
            {
                return ApiResponse.ErrorResponse("Category not found.");
            }

            var updatedCategory = mapper.Map<Category>(request.Request);

            if (category == updatedCategory)
            {
                return ApiResponse.ErrorResponse("No changes found.");
            }

            if (!string.IsNullOrEmpty(request.Request.Name))
            {
                category.Name = request.Request.Name;
            }

            if (!string.IsNullOrEmpty(request.Request.Url))
            {
                category.Url = request.Request.Url;
            }

            unitOfWork.CategoryRepository.Update(category);
            await unitOfWork.CompleteAsync();

            return ApiResponse.SuccessResponse("Coupon updated successfully.");
        }
    }
}
