using DigiDock.Base.Responses;
using MediatR;
using DigiDock.Schema.Responses;
using DigiDock.Schema.Requests;
using DigiDock.Data.Domain;

namespace DigiDock.Business.Cqrs;
public record GetAllCouponsQuery() : IRequest<ApiResponse<List<CouponResponse>>>;
public record GetActiveCouponsQuery() : IRequest<ApiResponse<List<CouponResponse>>>;

public record CreateCouponCommand(CouponCreateRequest Request) : IRequest<ApiResponse>;
public record UpdateCouponCommand(CouponUpdateRequest Request) : IRequest<ApiResponse>;
public record DeleteCouponCommand(long CouponId) : IRequest<ApiResponse>;

public record PublishCouponCodeCommand(long CouponId) : IRequest<ApiResponse>;