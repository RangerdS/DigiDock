using AutoMapper;
using DigiDock.Base.Responses;
using DigiDock.Business.Cqrs;
using DigiDock.Data.UnitOfWork;
using DigiDock.Schema.Responses;
using MediatR;

namespace DigiDock.Business.Query.CouponQueries
{
    public class GetActiveCouponsQueryHandler : IRequestHandler<GetActiveCouponsQuery, ApiResponse<List<CouponResponse>>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        public GetActiveCouponsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<ApiResponse<List<CouponResponse>>> Handle(GetActiveCouponsQuery request, CancellationToken cancellationToken)
        {
            var entityList = await unitOfWork.CouponRepository.Where(c => c.IsRedeemed == false);
            var mappedList = mapper.Map<List<CouponResponse>>(entityList);
            return ApiResponse<List<CouponResponse>>.SuccessResponse(mappedList);
        }
    }
}
