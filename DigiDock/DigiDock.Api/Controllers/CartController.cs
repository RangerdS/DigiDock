using DigiDock.Base.Responses;
using DigiDock.Business.Cqrs;
using DigiDock.Schema.Requests;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DigiDock.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController
    {
        private readonly IMediator mediator;

        public CartController(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpPost("AddToCart")]
        [Authorize(Roles = "admin, normal")]
        public async Task<ApiResponse> AddToCart([FromBody] AddToCartRequest request)
        {
            var operation = new AddToCartCommand(request);
            var result = await mediator.Send(operation);
            return result;
        }

        [HttpPost("RemoveFromCart")]
        [Authorize(Roles = "admin, normal")]
        public async Task<ApiResponse> RemoveFromCart([FromBody] RemoveFromCartRequest request)
        {
            var operation = new RemoveFromCartCommand(request);
            var result = await mediator.Send(operation);
            return result;
        }

        [HttpPost("ClearCart")]
        [Authorize(Roles = "admin, normal")]
        public async Task<ApiResponse> ClearCart()
        {
            var operation = new ClearCartCommand();
            var result = await mediator.Send(operation);
            return result;
        }

        [HttpGet("GetCart")]
        [Authorize(Roles = "admin, normal")]
        public async Task<ApiResponse> GetCart()
        {
            var operation = new GetCartQuery();
            var result = await mediator.Send(operation);
            return result;
        }

        [HttpPost("Checkout")]
        [Authorize(Roles = "admin, normal")]
        public async Task<ApiResponse> Checkout([FromBody] CheckoutRequest request)
        {
            var operation = new CheckoutCommand(request);
            var result = await mediator.Send(operation);
            return result;
        }

        [HttpPost("UpdateCartQuantity")]
        [Authorize(Roles = "admin, normal")]
        public async Task<ApiResponse> UpdateCartQuantity([FromBody] UpdateCartQuantityRequest request)
        {
            var operation = new UpdateCartQuantityCommand(request);
            var result = await mediator.Send(operation);
            return result;
        }
    }
}
