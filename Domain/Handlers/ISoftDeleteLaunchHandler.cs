using Domain.Commands.Launch.Requests;
using Domain.Commands.Launch.Responses;

namespace Domain.Handlers
{
    public interface ISoftDeleteLaunchHandler
    {
        Task<SoftDeleteLaunchResponse> Handle(SoftDeleteLaunchRequest request, CancellationToken cancellationToken);
    }
}