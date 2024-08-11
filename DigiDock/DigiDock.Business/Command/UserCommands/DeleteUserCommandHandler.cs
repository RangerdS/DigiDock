using DigiDock.Base.Helpers;
using DigiDock.Base.Responses;
using DigiDock.Business.Cqrs;
using DigiDock.Data.Domain;
using DigiDock.Data.UnitOfWork;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace DigiDock.Business.Command.UserCommands
{
    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, ApiResponse>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IHttpContextAccessor httpContextAccessor;
        public DeleteUserCommandHandler(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
        {
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        public async Task<ApiResponse> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var userIdClaim = httpContextAccessor.HttpContext?.User?.FindFirst("UserId")?.Value;
            if (userIdClaim is null)
            {
                return ApiResponse.ErrorResponse("User not found.");
            }
            var currentUserId = int.Parse(userIdClaim);

            var user = await unitOfWork.UserRepository.FirstOrDefault(u => u.Id == currentUserId);
            if (user is null)
            {
                return ApiResponse.ErrorResponse("User not found.");
            }

            await unitOfWork.UserRepository.Delete(user.Id);
            await unitOfWork.CompleteAsync();
            return ApiResponse.SuccessResponse("User deleted successfully");
        }
    }
}
