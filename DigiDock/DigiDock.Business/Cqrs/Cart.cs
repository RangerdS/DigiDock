using DigiDock.Base.Responses;
using MediatR;
using DigiDock.Schema.Responses;
using DigiDock.Schema.Requests;

namespace DigiDock.Business.Cqrs;

public record GetCartQuery() : IRequest<ApiResponse<List<CartResponse>>>;

public record AddToCartCommand(AddToCartRequest Request) : IRequest<ApiResponse>;
public record RemoveFromCartCommand(RemoveFromCartRequest Request) : IRequest<ApiResponse>;
public record ClearCartCommand() : IRequest<ApiResponse>;
public record CheckoutCommand(CheckoutRequest Request) : IRequest<ApiResponse>;
public record UpdateCartQuantityCommand(UpdateCartQuantityRequest Request) : IRequest<ApiResponse>;