using AutoMapper;
using DigiDock.Base.Responses;
using DigiDock.Business.Cqrs;
using DigiDock.Data.Domain;
using DigiDock.Data.UnitOfWork;
using DigiDock.Schema.Responses;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigiDock.Business.Query.CartQueries
{
    public class GetCartQueryHandler : IRequestHandler<GetCartQuery, ApiResponse<List<CartResponse>>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly IHttpContextAccessor httpContextAccessor;
        public GetCartQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        public async Task<ApiResponse<List<CartResponse>>> Handle(GetCartQuery request, CancellationToken cancellationToken)
        {
            var userIdClaim = httpContextAccessor.HttpContext?.User?.FindFirst("UserId")?.Value;
            if (userIdClaim is null)
            {
                return ApiResponse<List<CartResponse>>.ErrorResponse("User not found.");
            }
            var currentUserId = int.Parse(userIdClaim);

            var orderDetailList = await unitOfWork.OrderDetailRepository.Where(od => od.UserId == currentUserId && od.OrderId == null);

            var returnList = new List<CartResponse>();
            foreach (var orderDetail in orderDetailList)
            {
                var product = await unitOfWork.ProductRepository.GetByIdAsync(orderDetail.ProductId);
                if (product is null)
                {
                    continue;
                }
                var cartResponse = new CartResponse
                {
                    ProductId = product.Id,
                    ProductName = product.Name,
                    UnitPrice = product.Price,
                    Quantity = orderDetail.Quantity,
                    TotalPrice = product.Price * orderDetail.Quantity,
                    Id = orderDetail.Id
                };
                returnList.Add(cartResponse);
            }


            return ApiResponse<List<CartResponse>>.SuccessResponse(returnList);

        }
    }
}
