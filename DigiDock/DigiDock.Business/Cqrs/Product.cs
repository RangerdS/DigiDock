using DigiDock.Base.Responses;
using MediatR;
using DigiDock.Schema.Responses;
using DigiDock.Schema.Requests;

namespace DigiDock.Business.Cqrs;
public record GetAllProductQuery() : IRequest<ApiResponse<List<ProductResponse>>>;
public record GetProductByIdQuery(long ProductId) : IRequest<ApiResponse<ProductResponse>>;
public record GetProductByParametersQuery(string ProductName, string ProductCode) : IRequest<ApiResponse<List<ProductResponse>>>;
public record GetProductByCategoryQuery(long CategoryId) : IRequest<ApiResponse<List<ProductResponse>>>;

public record CreateProductCommand(ProductRequest Request) : IRequest<ApiResponse>;
public record UpdateProductCommand(long ProductId, ProductRequest Request) : IRequest<ApiResponse>;
public record DeleteProductCommand(long ProductId) : IRequest<ApiResponse>;