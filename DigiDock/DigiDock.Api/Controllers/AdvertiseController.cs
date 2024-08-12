using DigiDock.Base.Responses;
using DigiDock.Business.Cqrs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DigiDock.Api.Controllers
{
    [Route("admin/api/[controller]")]
    [ApiController]
    public class AdvertiseController
    {
        private readonly IMediator mediator;

        public AdvertiseController(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpPost("PublishCouponCode")]
        [Authorize(Roles = "admin")]
        public async Task<ApiResponse> PublishCouponCode([FromBody] long CouponId)
        {
            var operation = new PublishCouponCodeCommand(CouponId);
            var result = await mediator.Send(operation);
            return result;
        }

    }
}
