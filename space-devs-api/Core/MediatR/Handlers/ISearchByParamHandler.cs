using Core.CQRS.Queries.Launch.Requests;
using Core.CQRS.Queries.Launch.Responses;

namespace Core.MediatR.Handlers
{
    public interface ISearchByParamHandler
    {
        Task<SeachByParamResponse> Handle(SearchLaunchRequest request, CancellationToken cancellationToken);
    }
}