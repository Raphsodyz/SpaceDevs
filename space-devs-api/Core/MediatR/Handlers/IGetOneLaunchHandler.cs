using Core.CQRS.Queries.Launch.Requests;
using Core.CQRS.Queries.Launch.Responses;

namespace Core.MediatR.Handlers
{
    public interface IGetOneLaunchHandler
    {
        Task<GetOneLaunchResponse> Handle(GetByIdRequest request, CancellationToken cancellationToken);
    }
}