using Core.CQRS.Commands.Launch.Requests;
using Core.CQRS.Commands.Launch.Responses;

namespace Core.MediatR.Handlers
{
    public interface IUpdateDataSetHandler
    {
        Task<UpdateDataSetResponse> Handle(UpdateLaunchSetRequest request, CancellationToken cancellationToken);
    }
}