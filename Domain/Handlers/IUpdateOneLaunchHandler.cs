using Domain.Commands.Launch.Requests;
using Domain.Commands.Launch.Responses;

namespace Domain.Handlers
{
    public interface IUpdateOneLaunchHandler
    {
        Task<UpdateOneLaunchResponse> Handle(UpdateOneLaunchRequest request, CancellationToken cancellationToken);
    }
}