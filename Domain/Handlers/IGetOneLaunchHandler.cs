using Domain.Queries.Launch.Requests;
using Domain.Queries.Launch.Responses;

namespace Domain.Handlers
{
    public interface IGetOneLaunchHandler
    {
        Task<GetOneLaunchResponse> Handle(GetByIdRequest request, CancellationToken cancellationToken);
    }
}