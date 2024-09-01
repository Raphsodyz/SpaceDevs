using Core.CQRS.Commands.Launch.Requests;
using Core.CQRS.Commands.Launch.Responses;

namespace Core.ExternalServices
{
    public interface ISpaceDevsUpdateService
    {
        Task<UpdateDataSetResponse> UpdateLaunchSet(UpdateLaunchSetRequest request, CancellationToken cancellationToken);
        Task<UpdateOneLaunchResponse> UpdateLaunchById(Guid id, CancellationToken cancellationToken);
    }
}