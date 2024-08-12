using DigiDock.Base.Helpers;
using DigiDock.Base.Responses;
using DigiDock.Business.Cqrs;
using DigiDock.Data.Domain;
using DigiDock.Data.UnitOfWork;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace DigiDock.Business.Command.UserCommands
{
    public class UpdateUserPasswordCommandHandler : IRequestHandler<UpdateUserPasswordCommand, ApiResponse>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IHttpContextAccessor httpContextAccessor;
        public UpdateUserPasswordCommandHandler(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
        {
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        public async Task<ApiResponse> Handle(UpdateUserPasswordCommand request, CancellationToken cancellationToken)
        {
            var userIdClaim = httpContextAccessor.HttpContext?.User?.FindFirst("UserId")?.Value;
            if (userIdClaim is null)
            {
                return ApiResponse.ErrorResponse("User not found.");
            }
            var currentUserId = int.Parse(userIdClaim);

            var user = await unitOfWork.UserRepository.FirstOrDefault(u => u.Id == currentUserId);
            var userPassword = await unitOfWork.UserPasswordRepository.LastOrDefault(up => up.UserId == currentUserId);
            if(user is null)
            {
                return ApiResponse.ErrorResponse("User not found.");
            }
            else if(userPassword.Password != HashHelper.CreateMD5(request.Request.OldPassword))
            {
                return ApiResponse.ErrorResponse("Old Password does not match.");
            }

            await unitOfWork.UserPasswordRepository.Delete(userPassword.Id);

            await unitOfWork.UserPasswordRepository.InsertAsync(new UserPassword
            {
                UserId = user.Id,
                Password = HashHelper.CreateMD5(request.Request.NewPassword)
            });

            await unitOfWork.CompleteWithTransactionAsync();
            return ApiResponse.SuccessResponse("User password updated successfully");
        }
    }
}
