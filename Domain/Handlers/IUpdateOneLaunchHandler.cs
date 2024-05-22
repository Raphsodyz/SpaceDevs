using Domain.Commands.Launch.Responses;
using Domain.Shared.Request;

namespace Domain.Handlers
{
    public interface IUpdateOneLaunchHandler
    {
        Task<UpdateOneLaunchResponse> Handle(LaunchByIdRequest request, CancellationToken cancellationToken);
    }
}