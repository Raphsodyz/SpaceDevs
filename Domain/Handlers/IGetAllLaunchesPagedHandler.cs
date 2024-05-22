using Domain.Queries.Launch.Responses;
using Domain.Request;

namespace Domain.Handlers
{
    public interface IGetAllLaunchesPagedHandler
    {
        Task<GetLaunchesPagedResponse> Handle(PageRequest request, CancellationToken cancellationToken);
    }
}