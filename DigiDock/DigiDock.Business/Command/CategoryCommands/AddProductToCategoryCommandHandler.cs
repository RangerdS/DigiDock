using AutoMapper;
using DigiDock.Base.Responses;
using DigiDock.Business.Cqrs;
using DigiDock.Data.Domain;
using DigiDock.Data.UnitOfWork;
using MediatR;

namespace DigiDock.Business.Command.CategoryCommands
{
    public class AddProductToCategoryCommandHandler : IRequestHandler<AddProductToCategoryCommand, ApiResponse>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        public AddProductToCategoryCommandHandler(IUnitOfWork unitOfWork, IMapper mapper) 
        {
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<ApiResponse> Handle(AddProductToCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = await unitOfWork.CategoryRepository.FirstOrDefault(c => c.Id == request.Request.CategoryId);
            if(category is null)
            {
                return ApiResponse.ErrorResponse("Category not found");
            }

            var product = await unitOfWork.ProductRepository.FirstOrDefault(c => c.Id == request.Request.CategoryId);
            if (product is null)
            {
                return ApiResponse.ErrorResponse("Product not found");
            }

            await unitOfWork.ProductCategoryMapRepository.InsertAsync(new ProductCategoryMap
            {
                ProductId = product.Id,
                CategoryId = category.Id
            });

            return ApiResponse.SuccessResponse("Product added to Category successfully.");
        }
    }
}
