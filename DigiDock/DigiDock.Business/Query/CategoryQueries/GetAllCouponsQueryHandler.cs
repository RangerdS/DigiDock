using AutoMapper;
using DigiDock.Base.Responses;
using DigiDock.Business.Cqrs;
using DigiDock.Data.UnitOfWork;
using DigiDock.Schema.Responses;
using MediatR;

namespace DigiDock.Business.Query.CategoryQueries
{
    public class GetAllCategoryQueryHandler : IRequestHandler<GetAllCategoryQuery, ApiResponse<List<CategoryResponse>>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        public GetAllCategoryQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<ApiResponse<List<CategoryResponse>>> Handle(GetAllCategoryQuery request, CancellationToken cancellationToken)
        {
            var entityList = await unitOfWork.CategoryRepository.GetAllAsync();
            var mappedList = mapper.Map<List<CategoryResponse>>(entityList);
            return ApiResponse<List<CategoryResponse>>.SuccessResponse(mappedList);
        }
    }
}
