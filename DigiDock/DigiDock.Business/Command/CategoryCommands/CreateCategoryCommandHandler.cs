using AutoMapper;
using DigiDock.Base.Responses;
using DigiDock.Business.Cqrs;
using DigiDock.Data.Domain;
using DigiDock.Data.UnitOfWork;
using MediatR;

namespace DigiDock.Business.Command.CategoryCommands
{
    public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, ApiResponse>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        public CreateCategoryCommandHandler(IUnitOfWork unitOfWork, IMapper mapper) 
        {
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<ApiResponse> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = await unitOfWork.CategoryRepository.FirstOrDefault(c => c.Name == request.Request.Name);
            if(category is not null)
            {
                return ApiResponse.ErrorResponse("Category already exists");
            }

            var mappedCategory = mapper.Map<Category>(request);
            await unitOfWork.CategoryRepository.InsertAsync(mappedCategory);
            await unitOfWork.CompleteAsync();

            return ApiResponse.SuccessResponse("Category created successfully.");
        }
    }
}
