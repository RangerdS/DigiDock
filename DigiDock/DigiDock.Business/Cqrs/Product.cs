using DigiDock.Base.Responses;
using MediatR;
using DigiDock.Schema.Responses;
using DigiDock.Schema.Requests;

namespace DigiDock.Business.Cqrs;
public record GetAllProductQuery() : IRequest<ApiResponse<List<ProductResponse>>>;
public record GetProductsByCategoryQuery(ProductWithCategoryRequest Request) : IRequest<ApiResponse<List<ProductWithCategoryResponse>>>;

public record CreateProductCommand(ProductRequest Request) : IRequest<ApiResponse>;
public record UpdateProductCommand(ProductUpdateRequest Request) : IRequest<ApiResponse>;
public record DeleteProductCommand(long ProductId) : IRequest<ApiResponse>;