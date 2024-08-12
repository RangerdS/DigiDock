using DigiDock.Base.Responses;
using DigiDock.Business.Cqrs;
using DigiDock.Data.UnitOfWork;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace DigiDock.Business.Command.CartCommands
{
    public class ClearCartCommandHandler : IRequestHandler<ClearCartCommand, ApiResponse>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IHttpContextAccessor httpContextAccessor;
        public ClearCartCommandHandler(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
        {
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }


        public async Task<ApiResponse> Handle(ClearCartCommand request, CancellationToken cancellationToken)
        {
            var userIdClaim = httpContextAccessor.HttpContext?.User?.FindFirst("UserId")?.Value;
            if (userIdClaim is null)
            {
                return ApiResponse.ErrorResponse("User not found.");
            }
            var currentUserId = int.Parse(userIdClaim);

            var cartItems = await unitOfWork.OrderDetailRepository.Where(od => od.UserId == currentUserId && od.OrderId == null);
            if (!cartItems.Any())
            {
                return ApiResponse.ErrorResponse("Cart is already empty.");
            }

            foreach (var item in cartItems)
            {
                unitOfWork.OrderDetailRepository.Delete(item);
            }

            await unitOfWork.CompleteAsync();

            return ApiResponse.SuccessResponse("Cart cleared successfully.");
        }
    }
}
