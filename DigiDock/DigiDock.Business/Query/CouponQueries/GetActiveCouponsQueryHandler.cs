using AutoMapper;
using DigiDock.Base.Responses;
using DigiDock.Business.Cqrs;
using DigiDock.Data.Domain;
using DigiDock.Data.UnitOfWork;
using DigiDock.Schema.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigiDock.Business.Query.CouponsQueries
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
