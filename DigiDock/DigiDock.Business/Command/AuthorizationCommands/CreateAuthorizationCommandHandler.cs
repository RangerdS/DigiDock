using AutoMapper;
using DigiDock.Base.Helpers;
using DigiDock.Base.Responses;
using DigiDock.Business.Token;
using DigiDock.Data.Domain;
using DigiDock.Data.UnitOfWork;
using DigiDock.Schema.Responses;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DigiDock.Business.Cqrs.Authorization;

namespace DigiDock.Business.Command.AuthorizationCommands
{
    public class CreateAuthorizationCommandHandler : IRequestHandler<CreateAuthorizationTokenCommand, ApiResponse<AuthorizationResponse>>
    {
        private readonly IUnitOfWork unitOfWork;
        private ITokenService tokenService;

        public CreateAuthorizationCommandHandler(IUnitOfWork unitOfWork, ITokenService tokenService)
        {
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
        }

        public async Task<ApiResponse<AuthorizationResponse>> Handle(CreateAuthorizationTokenCommand request, CancellationToken cancellationToken)
        {
            var user = await unitOfWork.UserRepository.FirstOrDefault(x => x.Email == request.Request.Email, "UserPasswords");

            if (user is null || !IsPasswordValid(user, request.Request.Password) || !user.IsActive)
            {
                var errorMessage = user is null
                    ? "User not found."
                    : !IsPasswordValid(user, request.Request.Password)
                        ? "Password is wrong."
                        : "User is not active.";

                await LogFailedLogin(request, user?.Id ?? 1, errorMessage + $" Email: {request.Request.Email}");

                return ApiResponse<AuthorizationResponse>.ErrorResponse("Invalid user informations. Check your username or password.");
            }

            var token = await tokenService.GetToken(user);
            var response = new AuthorizationResponse
            {
                ExpireTime = DateTime.Now.AddMinutes(5),
                AccessToken = token,
                Email = user.Email
            };

            await LogSuccessfulLogin(request, user.Id);
            return ApiResponse<AuthorizationResponse>.SuccessResponse(response);
        }

        private async Task LogFailedLogin(CreateAuthorizationTokenCommand request, long userId, string errorMessage)
        {
            await unitOfWork.UserLoginRepository.InsertAsync(new UserLogin
            {
                IpAddress = request.IpAddress,
                IsLoginSuccess = false,
                ErrorMessage = errorMessage,
                UserId = userId
            });
            await unitOfWork.CompleteAsync();
        }

        private async Task LogSuccessfulLogin(CreateAuthorizationTokenCommand request, long userId)
        {
            await unitOfWork.UserLoginRepository.InsertAsync(new UserLogin
            {
                IpAddress = request.IpAddress,
                IsLoginSuccess = true,
                ErrorMessage = "Success",
                UserId = userId
            });
            await unitOfWork.CompleteAsync();
        }

        private bool IsPasswordValid(User user, string password)
        {
            return user.UserPasswords.LastOrDefault()?.Password == HashHelper.CreateMD5(password);
        }
    }
}
