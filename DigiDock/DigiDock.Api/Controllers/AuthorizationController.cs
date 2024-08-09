using DigiDock.Base.Responses;
using DigiDock.Schema.Requests;
using DigiDock.Schema.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static DigiDock.Business.Cqrs.Authorization;

namespace DigiDock.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorizationController
    {
        private readonly IMediator mediator;
        private readonly IHttpContextAccessor httpContextAccessor;

        public AuthorizationController(IMediator mediator, IHttpContextAccessor httpContextAccessor)
        {
            this.mediator = mediator;
            this.httpContextAccessor = httpContextAccessor;
        }


        [HttpPost("Login")]
        [AllowAnonymous]
        //fill here : [ResponseHeader("MyCustomHeaderInResponse", "POST")]
        public async Task<ApiResponse<AuthorizationResponse>> Login([FromBody] AuthorizationRequest value)
        {
            var operation = new CreateAuthorizationTokenCommand(value, httpContextAccessor.HttpContext.Connection.RemoteIpAddress?.ToString());
            var result = await mediator.Send(operation);
            return result;
        }
    }
}
