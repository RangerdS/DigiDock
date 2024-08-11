using AutoMapper;
using DigiDock.Base.Responses;
using DigiDock.Business.Cqrs;
using DigiDock.Data.Domain;
using DigiDock.Data.UnitOfWork;
using DigiDock.Schema.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DigiDock.Business.Command.ProductCommands
{
    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, ApiResponse>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        public UpdateProductCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<ApiResponse> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var product = await unitOfWork.ProductRepository.GetByIdAsync(request.Request.ProductId);
            if (product == null)
            {
                return ApiResponse.ErrorResponse("Product not found");
            }

            if (product == mapper.Map<Product>(request.Request))
            {
                return ApiResponse.ErrorResponse("No changes found");
            }

            if (!string.IsNullOrEmpty(request.Request.Name))
            {
                product.Name = request.Request.Name;
            }

            if (request.Request.Price.HasValue)
            {
                product.Price = request.Request.Price.Value;
            }

            if (!string.IsNullOrEmpty(request.Request.Description))
            {
                product.Description = request.Request.Description;
            }

            if (!string.IsNullOrEmpty(request.Request.Features))
            {
                product.Features = request.Request.Features;
            }

            if (request.Request.Stock.HasValue)
            {
                product.Stock = request.Request.Stock.Value;
            }

            if (request.Request.MaxRewardPoints.HasValue)
            {
                product.MaxRewardPoints = request.Request.MaxRewardPoints.Value;
            }

            if (request.Request.RewardPointsPercentage.HasValue)
            {
                product.RewardPointsPercentage = request.Request.RewardPointsPercentage.Value;
            }


            unitOfWork.ProductRepository.Update(product);
            await unitOfWork.CompleteAsync();

            return ApiResponse.ErrorResponse("Product updated successfully");
        }
    }
}
