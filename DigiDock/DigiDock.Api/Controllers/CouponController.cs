using DigiDock.Base.Responses;
using DigiDock.Business.Cqrs;
using DigiDock.Schema.Requests;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DigiDock.Api.Controllers
{
    [Route("admin/api/[controller]")]
    [ApiController]
    public class CouponController : ControllerBase
    {
        private readonly IMediator mediator;

        public CouponController(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpGet("GetAllCoupons")]
        [Authorize(Roles = "admin")]
        public async Task<ApiResponse> GetAllCoupons()
        {
            var operation = new GetAllCouponsQuery();
            var result = await mediator.Send(operation);
            return result;
        }

        [HttpGet("GetActiveCoupons")]
        [Authorize(Roles = "admin")]
        public async Task<ApiResponse> GetActiveCoupons()
        {
            var operation = new GetActiveCouponsQuery();
            var result = await mediator.Send(operation);
            return result;
        }

        [HttpPost("CreateCoupon")]
        [Authorize(Roles = "admin")]
        public async Task<ApiResponse> CreateCoupon([FromBody] CouponCreateRequest request)
        {
            var operation = new CreateCouponCommand(request);
            var result = await mediator.Send(operation);
            return result;
        }

        [HttpPut("UpdateCoupon")]
        [Authorize(Roles = "admin")]
        public async Task<ApiResponse> UpdateCoupon([FromBody] CouponUpdateRequest request)
        {
            var operation = new UpdateCouponCommand(request);
            var result = await mediator.Send(operation);
            return result;
        }

        [HttpDelete("DeleteCoupon")]
        [Authorize(Roles = "admin")]
        public async Task<ApiResponse> DeleteCoupon([FromBody] long CouponId)
        {
            var operation = new DeleteCouponCommand(CouponId);
            var result = await mediator.Send(operation);
            return result;
        }
    }
}
