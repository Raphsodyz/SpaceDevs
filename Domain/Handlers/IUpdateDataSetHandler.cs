using Domain.Commands.Launch.Requests;
using Domain.Commands.Launch.Responses;

namespace Domain.Handlers
{
    public interface IUpdateDataSetHandler
    {
        Task<UpdateDataSetResponse> Handle(UpdateLaunchRequest request, CancellationToken cancellationToken);
    }
}