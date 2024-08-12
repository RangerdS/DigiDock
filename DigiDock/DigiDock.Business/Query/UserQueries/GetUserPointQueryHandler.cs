using AutoMapper;
using DigiDock.Base.Responses;
using DigiDock.Business.Cqrs;
using DigiDock.Data.UnitOfWork;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace DigiDock.Business.Query.ProductQueries
{
    public class GetUserPointQueryHandler : IRequestHandler<GetUserPointQuery, ApiResponse<Decimal>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IHttpContextAccessor httpContextAccessor;
        public GetUserPointQueryHandler(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
        {
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        public async Task<ApiResponse<Decimal>> Handle(GetUserPointQuery request, CancellationToken cancellationToken)
        {
            var userIdClaim = httpContextAccessor.HttpContext?.User?.FindFirst("UserId")?.Value;
            if (userIdClaim is null)
            {
                return ApiResponse<Decimal>.ErrorResponse("User not found.");
            }
            var currentUserId = int.Parse(userIdClaim);

            var user = await unitOfWork.UserRepository.GetByIdAsync(currentUserId);
            var point = user.WalletBalance;
            return ApiResponse<Decimal>.SuccessResponse(point);
        }
    }
}
