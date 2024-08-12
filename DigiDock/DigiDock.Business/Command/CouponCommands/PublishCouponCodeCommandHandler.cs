using AutoMapper;
using DigiDock.Base.Helpers;
using DigiDock.Base.Responses;
using DigiDock.Business.Cqrs;
using DigiDock.Business.Services;
using DigiDock.Data.Domain;
using DigiDock.Data.UnitOfWork;
using DigiDock.Schema.Responses;
using MediatR;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigiDock.Business.Command.CouponCommands
{
    public class PublishCouponCodeCommandHandler : IRequestHandler<PublishCouponCodeCommand, ApiResponse>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly EmailQueueService emailQueueService;
        private readonly IConfiguration configuration;

        public PublishCouponCodeCommandHandler(IUnitOfWork unitOfWork, EmailQueueService emailQueueService, IConfiguration configuration)
        {
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.emailQueueService = emailQueueService ?? throw new ArgumentNullException(nameof(emailQueueService));
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public async Task<ApiResponse> Handle(PublishCouponCodeCommand request, CancellationToken cancellationToken)
        {
            var coupon = await unitOfWork.CouponRepository.FirstOrDefault(u => u.Id == request.CouponId);
            if (coupon is null)
            {
                return ApiResponse.ErrorResponse("Coupon not found");
            }

            string logoUrl = configuration["LogoUrl"];
            string emailBody = $@"
                <html>
                <body>
                    <div style='text-align: center; margin-bottom: 20px;'>
                        <img src='{logoUrl}' alt='DigiDock Logo' style='max-width: 100%; height: auto; width: 150px;' />
                    </div>
                    <h1>Congratulations!</h1>
                    <p>Dear Customer,</p>
                    <p>We are excited to inform you that a new coupon code has been published just for you.</p>
                    <p>Here are the details of your coupon:</p>
                    <ul>
                        <li><strong>Coupon Code:</strong> {coupon.Code}</li>
                        <li><strong>Discount:</strong> {coupon.Discount}%</li>
                        <li><strong>Valid Until:</strong> {coupon.ExpiryDate.ToString("MMMM dd, yyyy")}</li>
                    </ul>
                    <p>Use this coupon code at checkout to enjoy your discount.</p>
                    <p>Thank you for being a valued customer.</p>
                    <p>Best regards,</p>
                    <p>The DigiDock Team</p>
                </body>
                </html>";

            emailQueueService.EnqueueEmail(
                "Your New Coupon Code from DigiDock! " + coupon.Code.ToString(),
                emailBody);

            return ApiResponse.SuccessResponse("Coupon published successfully");
        }
    }
}
