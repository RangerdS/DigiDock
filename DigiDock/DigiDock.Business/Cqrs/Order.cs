using DigiDock.Base.Responses;
using MediatR;
using DigiDock.Schema.Responses;

namespace DigiDock.Business.Cqrs;

public record GetActiveOrdersQuery() : IRequest<ApiResponse<List<OrderResponse>>>;
public record GetOrderHistoryQuery() : IRequest<ApiResponse<List<OrderResponse>>>;