using AutoMapper;
using DigiDock.Base.Responses;
using DigiDock.Business.Cqrs;
using DigiDock.Data.Domain;
using DigiDock.Data.UnitOfWork;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace DigiDock.Business.Command.CartCommands
{
    public class AddToCartCommandHandler : IRequestHandler<AddToCartCommand, ApiResponse>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IHttpContextAccessor httpContextAccessor;
        public AddToCartCommandHandler(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
        {
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        public async Task<ApiResponse> Handle(AddToCartCommand request, CancellationToken cancellationToken)
        {
            var product = await unitOfWork.ProductRepository.FirstOrDefault(p => p.Id == request.Request.ProductId);
            if (product is null)
            {
                return ApiResponse.ErrorResponse("Product not found");
            }

            var userIdClaim = httpContextAccessor.HttpContext?.User?.FindFirst("UserId")?.Value;
            if (userIdClaim is null)
            {
                return ApiResponse.ErrorResponse("User not found.");
            }
            var currentUserId = int.Parse(userIdClaim);

            // if the product already in cart
            var onCart = await unitOfWork.OrderDetailRepository.FirstOrDefault(o => o.ProductId == request.Request.ProductId && o.OrderId == null && o.UserId == currentUserId);
            if (onCart is not null)
            {
                onCart.Quantity += request.Request.Quantity; 
                unitOfWork.OrderDetailRepository.Update(onCart);
                unitOfWork.CompleteAsync();

                return ApiResponse.SuccessResponse("Cart updated.");
            }
            
            await unitOfWork.OrderDetailRepository.InsertAsync(new OrderDetail
            {
                UserId = currentUserId,
                ProductId = product.Id,
                Quantity = request.Request.Quantity
            });

            await unitOfWork.CompleteAsync();

            return ApiResponse.SuccessResponse("Product added to cart.");
        }
    }
}
