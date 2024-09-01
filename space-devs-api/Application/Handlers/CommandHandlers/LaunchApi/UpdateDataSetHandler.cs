using Application.Wrappers;
using Core.CQRS.Commands.Launch.Requests;
using Core.CQRS.Commands.Launch.Responses;
using Core.ExternalServices;
using Core.MediatR.Handlers;
using MediatR;

namespace Application.Handlers.CommandHandlers.LaunchApi
{
    public class UpdateDataSetHandler(ISpaceDevsUpdateService service) : IRequestHandler<MediatrRequestWrapper<UpdateLaunchSetRequest, UpdateDataSetResponse>, UpdateDataSetResponse>, IUpdateDataSetHandler
    {
        private readonly ISpaceDevsUpdateService _service = service;
        public async Task<UpdateDataSetResponse> Handle(MediatrRequestWrapper<UpdateLaunchSetRequest, UpdateDataSetResponse> request, CancellationToken cancellationToken)
        {
            var domainRequest = request.DomainRequest;
            return await Handle(domainRequest, cancellationToken);
        }

        public async Task<UpdateDataSetResponse> Handle(UpdateLaunchSetRequest request, CancellationToken cancellationToken)
        {
            return await _service.UpdateLaunchSet(request, cancellationToken);
        }
    }
}