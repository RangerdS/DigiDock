using DigiDock.Base.Responses;
using DigiDock.Schema.Requests;
using DigiDock.Schema.Responses;
using MediatR;

namespace DigiDock.Business.Cqrs
{
    public class Authorization
    {
        public record CreateAuthorizationTokenCommand(AuthorizationRequest Request, string IpAddress) : IRequest<ApiResponse<AuthorizationResponse>>;
    }
}
