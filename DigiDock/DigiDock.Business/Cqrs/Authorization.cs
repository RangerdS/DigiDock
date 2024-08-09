using DigiDock.Base.Responses;
using DigiDock.Schema.Requests;
using DigiDock.Schema.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigiDock.Business.Cqrs
{
    public class Authorization
    {
        public record CreateAuthorizationTokenCommand(AuthorizationRequest Request, string IpAddress) : IRequest<ApiResponse<AuthorizationResponse>>;
    }
}
