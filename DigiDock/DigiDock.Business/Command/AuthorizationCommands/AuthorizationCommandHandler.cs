using AutoMapper;
using DigiDock.Base.Responses;
using DigiDock.Business.Token;
using DigiDock.Data.Domain;
using DigiDock.Data.UnitOfWork;
using DigiDock.Schema.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DigiDock.Business.Cqrs.Authorization;

namespace DigiDock.Business.Command.AuthorizationCommands
{
    public class AuthorizationCommandHandler : IRequestHandler<CreateAuthorizationTokenCommand, ApiResponse<AuthorizationResponse>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private ITokenService tokenService;

        public AuthorizationCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, ITokenService tokenService)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.tokenService = tokenService;
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

                await LogFailedLogin(request, user?.Id ?? 0, errorMessage + $" Email: {request.Request.Email}");

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
        }

        private bool IsPasswordValid(User user, string password)
        {
            return user.UserPasswords.LastOrDefault()?.Password == CreateMD5(password);
        }

        private string CreateMD5(string input)
        {
            using (var md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                return Convert.ToHexString(hashBytes).ToLower();
            }
        }
    }
}
