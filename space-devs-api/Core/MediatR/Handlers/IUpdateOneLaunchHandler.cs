using Core.CQRS.Commands.Launch.Requests;
using Core.CQRS.Commands.Launch.Responses;

namespace Core.MediatR.Handlers
{
    public interface IUpdateOneLaunchHandler
    {
        Task<UpdateOneLaunchResponse> Handle(UpdateOneLaunchRequest request, CancellationToken cancellationToken);
    }
}