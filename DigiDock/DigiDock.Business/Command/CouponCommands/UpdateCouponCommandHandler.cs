using AutoMapper;
using DigiDock.Base.Responses;
using DigiDock.Business.Cqrs;
using DigiDock.Data.Domain;
using DigiDock.Data.UnitOfWork;
using MediatR;

namespace DigiDock.Business.Command.CouponCommands
{
    public class UpdateCouponCommandHandler : IRequestHandler<UpdateCouponCommand, ApiResponse>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public UpdateCouponCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<ApiResponse> Handle(UpdateCouponCommand request, CancellationToken cancellationToken)
        {
            var coupon = await unitOfWork.CouponRepository.GetByIdAsync(request.Request.Id);
            if (coupon == null)
            {
                return ApiResponse.ErrorResponse("Coupon not found.");
            }

            var updatedCoupon = mapper.Map<Coupon>(request.Request);

            if (coupon == updatedCoupon)
            {
                return ApiResponse.ErrorResponse("No changes found.");
            }

            if (!string.IsNullOrEmpty(request.Request.Code))
            {
                coupon.Code = request.Request.Code;
            }

            if (request.Request.Discount.HasValue)
            {
                coupon.Discount = request.Request.Discount.Value;
            }

            if (request.Request.ExpiryDate.HasValue)
            {
                coupon.ExpiryDate = request.Request.ExpiryDate.Value;
            }

            if (request.Request.IsRedeemed.HasValue)
            {
                coupon.IsRedeemed = request.Request.IsRedeemed.Value;
            }

            unitOfWork.CouponRepository.Update(coupon);
            await unitOfWork.CompleteAsync();

            return ApiResponse.SuccessResponse("Coupon updated successfully.");
        }
    }
}
