﻿using DigiDock.Base.Responses;
using DigiDock.Business.Cqrs;
using DigiDock.Data.UnitOfWork;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace DigiDock.Business.Command.CartCommands
{
    public class RemoveFromCartCommandHandler : IRequestHandler<RemoveFromCartCommand, ApiResponse>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IHttpContextAccessor httpContextAccessor;
        public RemoveFromCartCommandHandler(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
        {
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        public async Task<ApiResponse> Handle(RemoveFromCartCommand request, CancellationToken cancellationToken)
        {
            var userIdClaim = httpContextAccessor.HttpContext?.User?.FindFirst("UserId")?.Value;
            if (userIdClaim is null)
            {
                return ApiResponse.ErrorResponse("User not found.");
            }
            var currentUserId = int.Parse(userIdClaim);

            var cartItem = await unitOfWork.OrderDetailRepository.FirstOrDefault(
                od => od.UserId == currentUserId && od.ProductId == request.Request.ProductId && od.OrderId == null);

            if (cartItem == null)
            {
                return ApiResponse.ErrorResponse("Product not found in cart.");
            }

            if (request.Request.Quantity >= cartItem.Quantity)
            {
                unitOfWork.OrderDetailRepository.Delete(cartItem);
            }
            else
            {
                cartItem.Quantity -= request.Request.Quantity;
                unitOfWork.OrderDetailRepository.Update(cartItem);
            }

            await unitOfWork.CompleteAsync();

            return ApiResponse.SuccessResponse("Product removed from cart successfully.");
        }
    }
}
