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
    public class CreateCouponCommandHandler : IRequestHandler<CreateCouponCommand, ApiResponse>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        public CreateCouponCommandHandler(IUnitOfWork unitOfWork, IMapper mapper) 
        {
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<ApiResponse> Handle(CreateCouponCommand request, CancellationToken cancellationToken)
        {
            var coupon = await unitOfWork.CouponRepository.FirstOrDefault(u => u.Code == request.Request.Code);
            if (coupon is not null)
            {
                return ApiResponse.ErrorResponse("Coupon already exists");
            }

            var mappedCoupon = mapper.Map<Coupon>(request.Request);
            await unitOfWork.CouponRepository.InsertAsync(mappedCoupon);
            await unitOfWork.CompleteAsync();

            return ApiResponse.SuccessResponse("Coupon created successfully");
        }
    }
}
