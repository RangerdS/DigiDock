using DigiDock.Base.Responses;
using DigiDock.Business.Cqrs;
using DigiDock.Schema.Requests;
using DigiDock.Schema.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DigiDock.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly IMediator mediator;

        public CategoryController(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }


        [HttpGet("GetAllCategories")]
        [Authorize(Roles = "admin, normal")]
        [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any)]
        public async Task<ApiResponse<List<CategoryResponse>>> GetAllCategories()
        {
            var operation = new GetAllCategoryQuery();
            var result = await mediator.Send(operation);
            return result;
        }

        [HttpPost("CreateCategory")]
        [Authorize(Roles = "admin")]
        public async Task<ApiResponse> CreateCategory([FromBody] CategoryRequest request)
        {
            var operation = new CreateCategoryCommand(request);
            var result = await mediator.Send(operation);
            return result;
        }

        [HttpPut("UpdateCategory")]
        [Authorize(Roles = "admin")]
        public async Task<ApiResponse> UpdateCategory([FromBody] CategoryUpdateRequest request)
        {
            var operation = new UpdateCategoryCommand(request);
            var result = await mediator.Send(operation);
            return result;
        }

        [HttpDelete("DeleteCategory")]
        [Authorize(Roles = "admin")]
        public async Task<ApiResponse> DeleteCategory([FromBody] long CategoryId)
        {
            var operation = new DeleteCategoryCommand(CategoryId);
            var result = await mediator.Send(operation);
            return result;
        }


        [HttpPost("AddProductToCategory")]
        [Authorize(Roles = "admin")]
        public async Task<ApiResponse> AddProductToCategory([FromBody] AddProductToCategoryRequest request)
        {
            var operation = new AddProductToCategoryCommand(request);
            var result = await mediator.Send(operation);
            return result;
        }
    }
}
