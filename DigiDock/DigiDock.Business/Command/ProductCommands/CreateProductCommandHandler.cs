using AutoMapper;
using DigiDock.Base.Responses;
using DigiDock.Business.Cqrs;
using DigiDock.Data.Domain;
using DigiDock.Data.UnitOfWork;
using MediatR;

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
            await unitOfWork.ProductRepository.InsertAsync(mapped);
            await unitOfWork.CompleteAsync(); 
            

            return ApiResponse.SuccessResponse();
        }
    }
}
