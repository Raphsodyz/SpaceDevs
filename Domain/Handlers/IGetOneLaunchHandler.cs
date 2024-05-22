using Domain.Queries.Launch.Responses;
using Domain.Shared.Request;

namespace Domain.Handlers
{
    public interface IGetOneLaunchHandler
    {
        Task<GetOneLaunchResponse> Handle(LaunchByIdRequest request, CancellationToken cancellationToken);
    }
}