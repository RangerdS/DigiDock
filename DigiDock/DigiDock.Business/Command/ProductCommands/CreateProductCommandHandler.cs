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
using System.Text;
using System.Threading.Tasks;

namespace DigiDock.Business.Command.ProductCommands
{
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, ApiResponse>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        public CreateProductCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<ApiResponse> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var mapped = mapper.Map<Product>(request.Request);
            await unitOfWork.ProductRepository.InsertAsync(mapped); // fill Here: if this is not logged then log it to db
            await unitOfWork.CompleteAsync(); // fill here: these DB insert update ex. should in rabbitmq // ? complete asycn hata aldığında kendi tekrar deniyor mu ??
            
            // fill here : product command'da cqrs'de tanımladığın tüm methodları tanımla


            return ApiResponse.SuccessResponse();
        }
    }
}
