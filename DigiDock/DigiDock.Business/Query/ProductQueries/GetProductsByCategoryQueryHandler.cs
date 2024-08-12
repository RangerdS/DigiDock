using DigiDock.Base.Responses;
using DigiDock.Business.Cqrs;
using DigiDock.Data.Domain;
using DigiDock.Data.UnitOfWork;
using DigiDock.Schema.Responses;
using MediatR;

namespace DigiDock.Business.Query.ProductQueries
{
    public class GetProductsByCategoryQueryHandler : IRequestHandler<GetProductsByCategoryQuery, ApiResponse<List<ProductWithCategoryResponse>>>
    {
        private readonly IUnitOfWork unitOfWork;
        public GetProductsByCategoryQueryHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<ApiResponse<List<ProductWithCategoryResponse>>> Handle(GetProductsByCategoryQuery request, CancellationToken cancellationToken)
        {
            return ApiResponse<List<ProductWithCategoryResponse>>.ErrorResponse(null);
        }
    }
}
