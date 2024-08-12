using DigiDock.Base.Responses;
using DigiDock.Business.Cqrs;
using DigiDock.Data.UnitOfWork;
using MediatR;

namespace DigiDock.Business.Command.CouponCommands
{
    public class DeleteCouponCommandHandler : IRequestHandler<DeleteCouponCommand, ApiResponse>
    {
        private readonly IUnitOfWork unitOfWork;
        public DeleteCouponCommandHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<ApiResponse> Handle(DeleteCouponCommand request, CancellationToken cancellationToken)
        {
            var coupon = await unitOfWork.CouponRepository.FirstOrDefault(c => c.Id == request.CouponId);
            if (coupon is null)
            {
                return ApiResponse.ErrorResponse("Coupon not found");
            }

            await unitOfWork.CouponRepository.Delete(coupon.Id);
            await unitOfWork.CompleteAsync();
            return ApiResponse.SuccessResponse("Coupon deleted successfully");
        }
    }
}
