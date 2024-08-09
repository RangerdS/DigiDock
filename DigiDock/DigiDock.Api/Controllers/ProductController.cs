using DigiDock.Base.Responses;
using DigiDock.Schema.Responses;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using DigiDock.Business.Cqrs;
using DigiDock.Schema.Requests;
using DigiDock.Api.Services;
using System;

namespace DigiDock.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly EmailQueueService emailQueueService;

        public ProductController(IMediator mediator, EmailQueueService emailQueueService)
        {
            this.mediator = mediator;
            this.emailQueueService = emailQueueService;
        }

        [HttpGet]
        public async Task<ApiResponse<List<ProductResponse>>> GetAllProducts()
        {
            var operation = new GetAllProductQuery();
            var result = await mediator.Send(operation);

            // fill here: delete under 3 line those are test
            var a = await mediator.Send(new GetAllUserEmailListQuery());

            Random random = new Random();
            emailQueueService.EnqueueEmailTo("info@sinansaglam.com.tr", "Confirmation Number", random.Next(100000, 1000000).ToString() + "\n" + result.Data.ToString());
            //fill here test ; emailQueueService.EnqueueEmail("Hello World", random.Next(100000, 1000000).ToString() + "\n" + result.Data.ToString());

            return result;
        }

        [HttpPost("CreateProduct")]
        public async Task<ApiResponse> CreateProduct([FromBody] ProductRequest request)
        {
            var operation = new CreateProductCommand(request);
            var result = await mediator.Send(operation);
            return result;
        }
    }
}
