using DigiDock.Base.Responses;
using MediatR;
using DigiDock.Schema.Responses;
using DigiDock.Schema.Requests;

namespace DigiDock.Business.Cqrs;
public record GetAllCategoryQuery() : IRequest<ApiResponse<List<CategoryResponse>>>;

public record CreateCategoryCommand(CategoryRequest Request) : IRequest<ApiResponse>;
public record AddProductToCategoryCommand(AddProductToCategoryRequest Request) : IRequest<ApiResponse>;
public record UpdateCategoryCommand(CategoryUpdateRequest Request) : IRequest<ApiResponse>;
public record DeleteCategoryCommand(long CategoryId) : IRequest<ApiResponse>;