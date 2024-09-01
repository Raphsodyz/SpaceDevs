using Core.CQRS.Queries.Launch.Requests;
using Core.CQRS.Queries.Launch.Responses;

namespace Core.MediatR.Handlers
{
    public interface IGetAllLaunchesPagedHandler
    {
        Task<GetLaunchesPagedResponse> Handle(PageRequest request, CancellationToken cancellationToken);
    }
}