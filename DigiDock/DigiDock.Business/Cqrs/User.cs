using DigiDock.Base.Responses;
using MediatR;
using DigiDock.Schema.Responses;
using DigiDock.Schema.Requests;

namespace DigiDock.Business.Cqrs;
public record GetAllUserEmailListQuery() : IRequest<ApiResponse<List<String>>>;