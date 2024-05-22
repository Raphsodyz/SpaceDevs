using Domain.Commands.Launch.Responses;
using Domain.Shared.Request;

namespace Domain.Handlers
{
    public interface ISoftDeleteLaunchHandler
    {
        Task<SoftDeleteLaunchResponse> Handle(LaunchByIdRequest request, CancellationToken cancellationToken);
    }
}