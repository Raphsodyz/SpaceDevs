using Core.CQRS.Commands.Launch.Requests;
using Core.CQRS.Commands.Launch.Responses;

namespace Core.MediatR.Handlers
{
    public interface ISoftDeleteLaunchHandler
    {
        Task<SoftDeleteLaunchResponse> Handle(SoftDeleteLaunchRequest request, CancellationToken cancellationToken);
    }
}