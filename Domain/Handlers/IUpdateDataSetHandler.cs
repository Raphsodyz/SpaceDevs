using Domain.Commands.Launch.Requests;
using Domain.Commands.Launch.Responses;

namespace Domain.Handlers
{
    public interface IUpdateDataSetHandler
    {
        Task<UpdateDataSetResponse> Handle(UpdateLaunchSetRequest request, CancellationToken cancellationToken);
    }
}