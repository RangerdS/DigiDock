using AutoMapper;
using DigiDock.Base.Responses;
using DigiDock.Business.Cqrs;
using DigiDock.Data.UnitOfWork;
using DigiDock.Schema.Responses;
using MediatR;

namespace DigiDock.Business.Query.CouponQueries
{
    public class GetAllCouponsQueryHandler : IRequestHandler<GetAllCouponsQuery, ApiResponse<List<CouponResponse>>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        public GetAllCouponsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<ApiResponse<List<CouponResponse>>> Handle(GetAllCouponsQuery request, CancellationToken cancellationToken)
        {
            var entityList = await unitOfWork.CouponRepository.GetAllAsync();
            var mappedList = mapper.Map<List<CouponResponse>>(entityList);
            return ApiResponse<List<CouponResponse>>.SuccessResponse(mappedList);
        }
    }
}
