using Application.Wrappers;
using Domain.Queries.Launch.Responses;
using Domain.Request;
using MediatR;

namespace Application.Handlers.QueryHandlers.Launch
{
    public class GetOneLaunchHandler : IRequestHandler<MediatrRequestWrapper<LaunchByIdRequest, GetOneLaunchResponse>, GetOneLaunchResponse>
    {
        public async Task<GetOneLaunchResponse> Handle(MediatrRequestWrapper<LaunchByIdRequest, GetOneLaunchResponse> request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}