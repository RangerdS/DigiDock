using DigiDock.Base.Responses;
using MediatR;
using DigiDock.Schema.Requests;

namespace DigiDock.Business.Cqrs;
public record GetAllUserEmailListQuery() : IRequest<ApiResponse<List<String>>>;
public record GetUserPointQuery() : IRequest<ApiResponse<Decimal>>;

public record CreateUserCommand(SignInRequest Request) : IRequest<ApiResponse>;
public record CreateUserWithRoleCommand(SignInRequest Request, string Role) : IRequest<ApiResponse>;
public record UpdateUserPasswordCommand(UserPasswordUpdateRequest Request) : IRequest<ApiResponse>;
public record DeleteUserCommand() : IRequest<ApiResponse>;