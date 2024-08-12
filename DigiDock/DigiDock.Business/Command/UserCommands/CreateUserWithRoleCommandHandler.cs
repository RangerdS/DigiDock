using AutoMapper;
using DigiDock.Base.Helpers;
using DigiDock.Base.Responses;
using DigiDock.Business.Cqrs;
using DigiDock.Business.Services;
using DigiDock.Data.Domain;
using DigiDock.Data.UnitOfWork;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace DigiDock.Business.Command.UserCommands
{
    public class CreateUserWithRoleCommandHandler : IRequestHandler<CreateUserWithRoleCommand, ApiResponse>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly EmailQueueService emailQueueService;
        private readonly IConfiguration configuration;
        public CreateUserWithRoleCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, EmailQueueService emailQueueService, IConfiguration configuration) 
        {
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.emailQueueService = emailQueueService ?? throw new ArgumentNullException(nameof(emailQueueService));
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration)); // Add this line
        }

        public async Task<ApiResponse> Handle(CreateUserWithRoleCommand request, CancellationToken cancellationToken)
        {
            var user = await unitOfWork.UserRepository.FirstOrDefault(u => u.Email == request.Request.Email);
            if (user is not null)
            {
                return ApiResponse.ErrorResponse("User already exists");
            }

            var mappedUser = mapper.Map<User>(request.Request);
            mappedUser.Role = request.Role;
            mappedUser.DigitalWalletInfo = Guid.NewGuid().ToString();
            mappedUser.WalletBalance = 0;
            await unitOfWork.UserRepository.InsertAsync(mappedUser);
            await unitOfWork.CompleteAsync();
            
            var mappedUserPassword = mapper.Map<UserPassword>(request.Request);
            mappedUserPassword.UserId = mappedUser.Id;
            mappedUserPassword.Password = HashHelper.CreateMD5(mappedUserPassword.Password);
            await unitOfWork.UserPasswordRepository.InsertAsync(mappedUserPassword);
            await unitOfWork.CompleteAsync();

            string logoUrl = configuration["LogoUrl"];
            string resetPasswordUrl = "https://localhost:7042/swagger/index.html";

            string emailBody = $@"
                <html>
                <body>
                    <div style='text-align: center; margin-bottom: 20px;'>
                        <img src='{logoUrl}' alt='DigiDock Logo' style='max-width: 100%; height: auto; width: 150px;' />
                    </div>
                    <h1>Welcome to DigiDock!</h1>
                    <p>Dear {mappedUser.FirstName} {mappedUser.LastName},</p>
                    <p>Your account has been created successfully. We are excited to have you on board.</p>
                    <p>Here are your account details:</p>
                    <ul>
                        <li><strong>Email:</strong> {request.Request.Email}</li>
                        <li><strong>Role:</strong> {mappedUser.Role}</li>
                        <li><strong>Digital Wallet Info:</strong> {mappedUser.DigitalWalletInfo}</li>
                    </ul>
                    <p>You can change your password using the following link:</p>
                    <p><a href='{resetPasswordUrl}'>Change Password</a></p>                    
                    <p>Feel free to explore our platform and let us know if you have any questions.</p>
                    <p>Best regards,</p>
                    <p>The DigiDock Team</p>
                </body>
                </html>";

            emailQueueService.EnqueueEmailTo(
                request.Request.Email,
                "Welcome to DigiDock! Let's get started",
                emailBody);

            return ApiResponse.SuccessResponse("User created successfully");
        }
    }
}
