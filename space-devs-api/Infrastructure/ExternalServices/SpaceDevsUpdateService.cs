using Core.CQRS.Commands.Launch.Requests;
using Core.CQRS.Commands.Launch.Responses;
using Core.ExternalServices;
using Flurl.Http;

namespace Infrastructure.ExternalServices
{
    public class SpaceDevsUpdateService : ISpaceDevsUpdateService
    {
        private readonly string spaceDevsPublisherUrl = Environment.GetEnvironmentVariable("");

        public async Task<UpdateOneLaunchResponse> UpdateLaunchById(Guid launchId, CancellationToken cancellationToken)
        {
            return await spaceDevsPublisherUrl
                .PostJsonAsync(new { launchId }, cancellationToken: cancellationToken)
                .ReceiveJson<UpdateOneLaunchResponse>();
        }

        public async Task<UpdateDataSetResponse> UpdateLaunchSet(UpdateLaunchSetRequest request, CancellationToken cancellationToken)
        {
            return await spaceDevsPublisherUrl
                .PostJsonAsync(request, cancellationToken: cancellationToken)
                .ReceiveJson<UpdateDataSetResponse>();
        }
    }
}