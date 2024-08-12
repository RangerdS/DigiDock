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
    public class UserController : ControllerBase
    {
        private readonly IMediator mediator;

        public UserController(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }
        
        [HttpPut("UpdateUserPassword")]
        [Authorize(Roles = "admin, normal")]
        public async Task<ApiResponse> UpdateUserPassword([FromBody] UserPasswordUpdateRequest request)
        {
            var operation = new UpdateUserPasswordCommand(request);
            var result = await mediator.Send(operation);
            return result;
        }

        [HttpDelete("DeleteUser")]
        [Authorize(Roles = "admin, normal")]
        public async Task<ApiResponse> DeleteUser()
        {
            var operation = new DeleteUserCommand();
            var result = await mediator.Send(operation);
            return result;
        }


        [HttpGet("GetUserPoint")]
        [Authorize(Roles = "admin, normal")]
        public async Task<ApiResponse> GetUserPoint()
        {
            var operation = new GetUserPointQuery();
            var result = await mediator.Send(operation);
            return result;
        }
    }
}
