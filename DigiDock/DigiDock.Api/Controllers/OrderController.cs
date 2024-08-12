using DigiDock.Base.Responses;
using DigiDock.Business.Cqrs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DigiDock.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController
    {
        private readonly IMediator mediator;

        public OrderController(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpGet("active")]
        [Authorize(Roles = "admin, normal")]
        public async Task<ApiResponse> GetActiveOrders()
        {
            var operation = new GetActiveOrdersQuery();
            var result = await mediator.Send(operation);
            return result;
        }

        [HttpGet("history")]
        [Authorize(Roles = "admin, normal")]
        public async Task<ApiResponse> GetOrderHistory()
        {
            var operation = new GetOrderHistoryQuery();
            var result = await mediator.Send(operation);
            return result;
        }
    }
}