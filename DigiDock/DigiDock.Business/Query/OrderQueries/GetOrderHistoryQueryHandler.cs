using AutoMapper;
using DigiDock.Base.Responses;
using DigiDock.Business.Cqrs;
using DigiDock.Data.UnitOfWork;
using DigiDock.Schema.Responses;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace DigiDock.Business.Query.ProductQueries
{
    public class GetOrderHistoryQueryHandler : IRequestHandler<GetOrderHistoryQuery, ApiResponse<List<OrderResponse>>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly IHttpContextAccessor httpContextAccessor;

        public GetOrderHistoryQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        public async Task<ApiResponse<List<OrderResponse>>> Handle(GetOrderHistoryQuery request, CancellationToken cancellationToken)
        {
            var userIdClaim = httpContextAccessor.HttpContext?.User?.FindFirst("UserId")?.Value;
            if (userIdClaim is null)
            {
                return ApiResponse<List<OrderResponse>>.ErrorResponse("User not found.");
            }
            var currentUserId = int.Parse(userIdClaim);

            var activeOrders = await unitOfWork.OrderRepository
                .Where(o => o.UserId == currentUserId && o.IsActive == true, "OrderDetails");

            var orderResponses = new List<OrderResponse>();

            foreach (var order in activeOrders)
            {
                var orderResponse = new OrderResponse
                {
                    OrderId = order.Id,
                    UserId = order.UserId,
                    OrderDate = order.CreatedAt,
                    Status = order.IsActive ? "Active" : "Inactive",
                    Items = new List<OrderItemResponse>()
                };

                foreach (var detail in order.OrderDetails)
                {
                    var product = await unitOfWork.ProductRepository.GetByIdAsync(detail.ProductId);
                    var orderItemResponse = new OrderItemResponse
                    {
                        ProductId = detail.ProductId,
                        ProductName = product?.Name ?? "Unknown",
                        Quantity = detail.Quantity,
                        Price = detail.UnitPrice ?? 0
                    };
                    orderResponse.Items.Add(orderItemResponse);
                }

                orderResponses.Add(orderResponse);
            }

            return ApiResponse<List<OrderResponse>>.SuccessResponse(orderResponses);
        }
    }
}
