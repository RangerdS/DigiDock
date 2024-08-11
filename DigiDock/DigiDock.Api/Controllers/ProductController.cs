using DigiDock.Base.Responses;
using DigiDock.Schema.Responses;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using DigiDock.Business.Cqrs;
using DigiDock.Schema.Requests;
using DigiDock.Business.Services;
using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;

namespace DigiDock.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IMediator mediator;

        public ProductController(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpGet("GetAllProducts")]
        [Authorize(Roles = "admin, normal")]
        [ResponseCache(Duration = 600, Location = ResponseCacheLocation.Any)]
        public async Task<ApiResponse<List<ProductResponse>>> GetAllProducts()
        {
            var operation = new GetAllProductQuery();
            var result = await mediator.Send(operation);
            return result;
        }

        [HttpPost("CreateProduct")]
        [Authorize(Roles = "admin")]
        public async Task<ApiResponse> CreateProduct([FromBody] ProductRequest request)
        {
            var operation = new CreateProductCommand(request);
            var result = await mediator.Send(operation);
            return result;
        }

        [HttpPut("UpdateProduct")]
        [Authorize(Roles = "admin")]
        public async Task<ApiResponse> UpdateProduct([FromBody] ProductUpdateRequest request)
        {
            var operation = new UpdateProductCommand(request);
            var result = await mediator.Send(operation);
            return result;
        }

        [HttpDelete("DeleteProduct")]
        [Authorize(Roles = "admin")]
        public async Task<ApiResponse> DeleteProduct([FromBody] long request)
        {
            var operation = new DeleteProductCommand(request);
            var result = await mediator.Send(operation);
            return result;
        }
    }
}
