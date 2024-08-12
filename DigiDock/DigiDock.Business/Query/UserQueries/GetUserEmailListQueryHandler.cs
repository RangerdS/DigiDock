using AutoMapper;
using DigiDock.Base.Responses;
using DigiDock.Business.Cqrs;
using DigiDock.Data.UnitOfWork;
using MediatR;

namespace DigiDock.Business.Query.ProductQueries
{
    public class GetAllUserEmailListQueryHandler : IRequestHandler<GetAllUserEmailListQuery, ApiResponse<List<String>>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        public GetAllUserEmailListQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<ApiResponse<List<String>>> Handle(GetAllUserEmailListQuery request, CancellationToken cancellationToken)
        {
            var entityList = await unitOfWork.UserRepository.GetAllAsync();
            var emailList = entityList.Select(x => x.Email).ToList();
            return ApiResponse<List<String>>.SuccessResponse(emailList);
        }
    }
}
