using Domain.Queries.Launch.Responses;
using Domain.Request;

namespace Domain.Handlers
{
    public interface ISearchByParamHandler
    {
        Task<SeachByParamResponse> Handle(SearchLaunchRequest request, CancellationToken cancellationToken);
    }
}